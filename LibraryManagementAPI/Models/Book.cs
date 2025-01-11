using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementAPI.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = "";

        [Required]
        public string ISBN { get; set; } = "";

        public int PublishedYear { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }

        public Author? Author { get; set; }
    }
}
