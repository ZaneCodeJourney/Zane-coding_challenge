using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add unique constraint for ISBN in Book
            modelBuilder.Entity<Book>().HasIndex(b => b.ISBN).IsUnique();

            // Initialize the database using seed data
            modelBuilder.Entity<Author>().HasData(SeedData.Authors);
            modelBuilder.Entity<Book>().HasData(SeedData.Books);
        }
    }
}
