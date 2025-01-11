using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public AuthorsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            var authors = await _context.Authors.ToListAsync();

            if (authors.Count == 0)
            {
                return NotFound("No authors found.");
            }

            return Ok(authors);
        }

        // GET: api/Authors/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid author ID.");
            }

            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound($"Author with ID {id} not found.");
            }

            return Ok(author);
        }

        // POST: api/Authors
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check for duplicate author name
            if (_context.Authors.Any(a => a.Name.Equals(author.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return Conflict($"An author with the name '{author.Name}' already exists.");
            }

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }

        // PUT: api/Authors/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
            if (id <= 0 || id != author.Id)
            {
                return BadRequest("The ID in the URL must match the ID in the request body.");
            }

            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Authors.Any(e => e.Id == id))
                {
                    return NotFound($"Author with ID {id} not found.");
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Authors/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid author ID.");
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound($"Author with ID {id} not found.");
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
