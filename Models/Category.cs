using System.ComponentModel.DataAnnotations;

namespace OnlineBookStoreAPI.Models
{
    public class Category
    {
        [Key]
        public required string Name { get; set; }


        public ICollection<Book>? Books { get; set; }
    }
}
