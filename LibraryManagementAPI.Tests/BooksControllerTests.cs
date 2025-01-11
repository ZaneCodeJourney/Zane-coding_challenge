using FluentAssertions;
using LibraryManagementAPI.Controllers;
using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Tests
{
    [TestClass]
    public class BooksControllerTests
    {
        private Mock<LibraryContext> _mockContext;
        private BooksController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            // Mock Books DbSet
            var mockBooks = new List<Book>
            {
                new Book { Id = 1, Title = "1984", ISBN = "1234567890", PublishedYear = 1949, AuthorId = 1 },
                new Book { Id = 2, Title = "Harry Potter", ISBN = "0987654321", PublishedYear = 1997, AuthorId = 2 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Book>>();
            mockSet.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(mockBooks.Provider);
            mockSet.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(mockBooks.Expression);
            mockSet.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(mockBooks.ElementType);
            mockSet.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(mockBooks.GetEnumerator());

            // Mock LibraryContext
            _mockContext = new Mock<LibraryContext>();
            _mockContext.Setup(c => c.Books).Returns(mockSet.Object);

            // Initialize controller
            _controller = new BooksController(_mockContext.Object);
        }

        [TestMethod]
        public async Task GetBooks_ShouldReturnAllBooks()
        {
            // Act
            var result = await _controller.GetBooks();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var books = okResult.Value as IEnumerable<Book>;
            books.Should().NotBeNull();
            books.Count().Should().Be(2); // Mocked data contains 2 books
        }

        [TestMethod]
        public async Task GetBook_WithValidId_ShouldReturnBook()
        {
            // Act
            var result = await _controller.GetBook(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var book = okResult.Value as Book;
            book.Should().NotBeNull();
            book.Id.Should().Be(1);
            book.Title.Should().Be("1984");
        }
    }
}
