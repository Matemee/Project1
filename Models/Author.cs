using System.ComponentModel.DataAnnotations;

namespace OnlineBookStoreAPI.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }


        public ICollection<Book>? Books { get; set; }
    }
}
