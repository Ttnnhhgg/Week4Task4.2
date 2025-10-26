using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Course4.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Название книги обязательно")]
        [StringLength(200, ErrorMessage = "Название книги не может превышать 200 символов")]
        public string Title { get; set; } = string.Empty;
        [Range(1000, 2100, ErrorMessage = "Год публикации должен быть между 1000 и 2100")]
        public int PublishedYear { get; set; }      
        public int AuthorId { get; set; }      
        [ForeignKey("AuthorId")]
        public virtual Author? Author { get; set; }
    }
}