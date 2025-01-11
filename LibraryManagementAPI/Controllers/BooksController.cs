using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest(new { Message = "Page and pageSize must be greater than 0." });
            }

            var totalBooks = await _context.Books.CountAsync();
            var books = await _context
                .Books.Include(b => b.Author)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var paginationMetadata = new
            {
                TotalCount = totalBooks,
                PageSize = pageSize,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalBooks / (double)pageSize)
            };

            Response.Headers.Add(
                "X-Pagination",
                System.Text.Json.JsonSerializer.Serialize(paginationMetadata)
            );

            return Ok(books);
        }

        // GET: api/Books/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context
                .Books.Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound(new { Message = $"Book with ID {id} not found." });
            }

            return Ok(book);
        }

        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }

            // Check for duplicate ISBN
            if (_context.Books.Any(b => b.ISBN == book.ISBN))
            {
                return Conflict(new { Message = "A book with the same ISBN already exists." });
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // PUT: api/Books/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest(
                    new { Message = "The ID in the URL does not match the ID in the body." }
                );
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }

            var existingBook = await _context.Books.FindAsync(id);
            if (existingBook == null)
            {
                return NotFound(new { Message = $"Book with ID {id} not found." });
            }

            existingBook.Title = book.Title;
            existingBook.ISBN = book.ISBN;
            existingBook.PublishedYear = book.PublishedYear;
            existingBook.AuthorId = book.AuthorId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(
                    new { Message = "A concurrency issue occurred while updating the book." }
                );
            }

            return NoContent();
        }

        // DELETE: api/Books/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound(new { Message = $"Book with ID {id} not found." });
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: /api/books/search?title=xyz
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Book>>> SearchBooks([FromQuery] string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return BadRequest(new { Message = "The title query parameter is required." });
            }

            var books = await _context
                .Books.Include(b => b.Author)
                .Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            if (!books.Any())
            {
                return NotFound(
                    new { Message = $"No books found with title containing '{title}'." }
                );
            }

            return Ok(books);
        }
    }
}
