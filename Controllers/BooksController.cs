using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Course4.Data;
using Course4.Models;
namespace Course4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;
        public BooksController(LibraryContext context)
        {
            _context = context;
        }        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            try
            {                
                var books = await _context.Books
                    .Include(b => b.Author)
                    .ToListAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }       
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            try
            {
                var book = await _context.Books
                    .Include(b => b.Author)
                    .FirstOrDefaultAsync(b => b.Id == id);
                if (book == null)
                {
                    return NotFound($"Книга с ID {id} не найдена");
                }
                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }        
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook(Book book)
        {
            try
            {               
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (string.IsNullOrWhiteSpace(book.Title))
                {
                    return BadRequest("Название книги обязательно для заполнения");
                }
                if (book.PublishedYear < 1000 || book.PublishedYear > DateTime.Now.Year)
                {
                    return BadRequest($"Год публикации должен быть между 1000 и {DateTime.Now.Year}");
                }                
                var authorExists = await _context.Authors.AnyAsync(a => a.Id == book.AuthorId);
                if (!authorExists)
                {
                    return BadRequest("Автор с указанным ID не существует");
                }
                _context.Books.Add(book);
                await _context.SaveChangesAsync();                
                var createdBook = await _context.Books
                    .Include(b => b.Author)
                    .FirstOrDefaultAsync(b => b.Id == book.Id);
                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, createdBook);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при создании книги: {ex.Message}");
            }
        }      
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, Book book)
        {
            try
            {
                if (id != book.Id)
                {
                    return BadRequest("ID в URL не совпадает с ID в теле запроса");
                }
                var existingBook = await _context.Books.FindAsync(id);
                if (existingBook == null)
                {
                    return NotFound($"Книга с ID {id} не найдена");
                }                
                if (string.IsNullOrWhiteSpace(book.Title))
                {
                    return BadRequest("Название книги обязательно для заполнения");
                }
                if (book.PublishedYear < 1000 || book.PublishedYear > DateTime.Now.Year)
                {
                    return BadRequest($"Год публикации должен быть между 1000 и {DateTime.Now.Year}");
                }                
                var authorExists = await _context.Authors.AnyAsync(a => a.Id == book.AuthorId);
                if (!authorExists)
                {
                    return BadRequest("Автор с указанным ID не существует");
                }                
                existingBook.Title = book.Title.Trim();
                existingBook.PublishedYear = book.PublishedYear;
                existingBook.AuthorId = book.AuthorId;
                _context.Entry(existingBook).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при обновлении книги: {ex.Message}");
            }
        }       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return NotFound($"Книга с ID {id} не найдена");
                }
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при удалении книги: {ex.Message}");
            }
        }         
        [HttpGet("byauthor/{authorId}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByAuthor(int authorId)
        {
            try
            {                
                var author = await _context.Authors.FindAsync(authorId);
                if (author == null)
                {
                    return NotFound($"Автор с ID {authorId} не найден");
                }
                var books = await _context.Books
                    .Where(b => b.AuthorId == authorId)
                    .Include(b => b.Author)
                    .ToListAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }        
        [HttpGet("recent")]
        public async Task<ActionResult<IEnumerable<Book>>> GetRecentBooks()
        {
            try
            {               
                var recentBooks = await _context.Books
                    .Where(b => b.PublishedYear > 2015)
                    .Include(b => b.Author)
                    .OrderByDescending(b => b.PublishedYear)
                    .ToListAsync();
                return Ok(recentBooks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при получении книг: {ex.Message}");
            }
        }       
        [HttpGet("search/{title}")]
        public async Task<ActionResult<IEnumerable<Book>>> SearchBooks(string title)
        {
            try
            {                
                var books = await _context.Books
                    .Where(b => b.Title.Contains(title))
                    .Include(b => b.Author)
                    .ToListAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при поиске книг: {ex.Message}");
            }
        }       
        [HttpGet("search-start/{title}")]
        public async Task<ActionResult<IEnumerable<Book>>> SearchBooksStartsWith(string title)
        {
            try
            {               
                var books = await _context.Books
                    .Where(b => b.Title.StartsWith(title))
                    .Include(b => b.Author)
                    .ToListAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при поиске книг: {ex.Message}");
            }
        }
        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
