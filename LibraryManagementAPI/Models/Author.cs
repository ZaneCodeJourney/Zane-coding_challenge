using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        public DateTime DateOfBirth { get; set; }

        public string Biography { get; set; } = "";
    }
}
