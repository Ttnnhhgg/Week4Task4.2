using System.ComponentModel.DataAnnotations;
namespace Course4.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}