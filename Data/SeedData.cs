using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Data
{
    public static class SeedData
    {
        public static readonly List<Author> Authors = new List<Author>
        {
            new Author
            {
                Id = 1,
                Name = "George Orwell",
                DateOfBirth = new DateTime(1903, 6, 25)
            },
            new Author
            {
                Id = 2,
                Name = "J.K. Rowling",
                DateOfBirth = new DateTime(1965, 7, 31)
            }
        };

        public static readonly List<Book> Books = new List<Book>
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
                Title = "Harry Potter and the Philosopher's Stone",
                ISBN = "0987654321",
                PublishedYear = 1997,
                AuthorId = 2
            }
        };

        public static void Initialize(IServiceProvider serviceProvider)
        {
            // Get the LibraryContext instance directly from the dependency injection container
            using var context = serviceProvider.GetRequiredService<LibraryContext>();

            // If there is already data, no more insertion will be done.
            if (context.Books.Any() || context.Authors.Any())
                return;

            // Insert seed data
            context.Authors.AddRange(Authors);
            context.Books.AddRange(Books);
            context.SaveChanges();
        }
    }
}
