using System.ComponentModel.DataAnnotations;

namespace OnlineBookStoreAPI.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [MaxLength(20)]
        public string Price { get; set; }

        [Required]
        public DateTime PublicationDate { get; set; }


        [Required]
        public ICollection<Category> Categories { get; set; }

        [Required]
        public ICollection<Author> Authors { get; set; }
    }
}
