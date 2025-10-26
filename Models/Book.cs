using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Course4.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        [Range(1000, 2100)]
        public int PublishedYear { get; set; }        
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual Author? Author { get; set; }
    }
}