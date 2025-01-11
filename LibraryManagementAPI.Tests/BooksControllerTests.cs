using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LibraryManagementAPI.Controllers;
using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibraryManagementAPI.Tests
{
    [TestClass]
    public class BooksControllerTests
    {
        private BooksController _controller;
        private LibraryContext _context;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: "LibraryTestDb")
                .Options;

            _context = new LibraryContext(options);

            // Seed test data
            _context.Books.AddRange(
                new List<Book>
                {
                    new Book
                    {
                        Id = 1,
                        Title = "1984",
                        ISBN = "1234567890",
                        PublishedYear = 1949,
                        AuthorId = 1
                    },
                    new Book
                    {
                        Id = 2,
                        Title = "Harry Potter",
                        ISBN = "0987654321",
                        PublishedYear = 1997,
                        AuthorId = 2
                    }
                }
            );
            _context.SaveChanges();

            _controller = new BooksController(_context);
        }

        [TestMethod]
        public async Task GetBooks_ShouldReturnAllBooks()
        {
            // Act
            var result = await _controller.GetBooks();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            var books = okResult.Value as IEnumerable<Book>;
            books.Should().NotBeNull();
            books.Count().Should().Be(2);
        }

        [TestMethod]
        public async Task GetBook_WithInvalidId_ShouldReturnNotFound()
        {
            // Act
            var result = await _controller.GetBook(99);

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.Value.Should().Be("Book with ID 99 not found.");
        }
    }
}
