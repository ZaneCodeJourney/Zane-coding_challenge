using System;
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
    public class AuthorsControllerTests
    {
        private AuthorsController _controller = null!;
        private LibraryContext _context = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            // Use InMemoryDatabase to create a unique database instance
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new LibraryContext(options);

            // Add test data
            _context.Authors.AddRange(
                new List<Author>
                {
                    new Author
                    {
                        Id = 1,
                        Name = "George Orwell",
                        DateOfBirth = new DateTime(1903, 6, 25),
                        Biography = "Dystopian Novelist"
                    },
                    new Author
                    {
                        Id = 2,
                        Name = "J.K. Rowling",
                        DateOfBirth = new DateTime(1965, 7, 31),
                        Biography = "Fantasy Novelist"
                    }
                }
            );

            _context.SaveChanges();

            // Initialize AuthorsController
            _controller = new AuthorsController(_context);
        }

        [TestMethod]
        public async Task GetAuthors_ShouldReturnAllAuthors()
        {
            // Act
            var result = await _controller.GetAuthors();

            // Assert
            result.Value.Should().NotBeNull();
            result.Value!.Count().Should().Be(2); // Should return 2 authors
        }

        [TestMethod]
        public async Task GetAuthor_WithInvalidId_ShouldReturnNotFound()
        {
            // Act
            var result = await _controller.GetAuthor(99);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>(); // Should return NotFoundResult
        }
    }
}
