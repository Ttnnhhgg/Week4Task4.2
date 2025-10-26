using System.ComponentModel.DataAnnotations;
namespace Course4.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Имя автора обязательно")]
        [StringLength(100, ErrorMessage = "Имя автора не может превышать 100 символов")]
        public string Name { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Дата рождения обязательна")]
        public DateTime DateOfBirth { get; set; }        
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}