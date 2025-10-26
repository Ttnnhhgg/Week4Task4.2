using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Course4.Data;
using Course4.Models;
namespace Course4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly LibraryContext _context;       
        public AuthorsController(LibraryContext context)
        {
            _context = context;
        }     
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            try
            {              
                var authors = await _context.Authors
                    .Include(a => a.Books)
                    .ToListAsync();
                return Ok(authors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            try
            {               
                var author = await _context.Authors
                    .Include(a => a.Books)
                    .FirstOrDefaultAsync(a => a.Id == id);
                if (author == null)
                {
                    return NotFound($"Автор с ID {id} не найден");
                }
                return Ok(author);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }      
        [HttpPost]
        public async Task<ActionResult<Author>> CreateAuthor(Author author)
        {
            try
            {               
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (string.IsNullOrWhiteSpace(author.Name))
                {
                    return BadRequest("Имя автора обязательно для заполнения");
                }
                if (author.DateOfBirth > DateTime.Now)
                {
                    return BadRequest("Дата рождения не может быть в будущем");
                }             
                _context.Authors.Add(author);                
                await _context.SaveChangesAsync();                
                return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при создании автора: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, Author author)
        {
            try
            {
                if (id != author.Id)
                {
                    return BadRequest("ID в URL не совпадает с ID в теле запроса");
                }                
                var existingAuthor = await _context.Authors.FindAsync(id);
                if (existingAuthor == null)
                {
                    return NotFound($"Автор с ID {id} не найден");
                }              
                if (string.IsNullOrWhiteSpace(author.Name))
                {
                    return BadRequest("Имя автора обязательно для заполнения");
                }
                if (author.DateOfBirth > DateTime.Now)
                {
                    return BadRequest("Дата рождения не может быть в будущем");
                }               
                existingAuthor.Name = author.Name.Trim();
                existingAuthor.DateOfBirth = author.DateOfBirth;                
                _context.Entry(existingAuthor).State = EntityState.Modified;              
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
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
                return StatusCode(500, $"Ошибка при обновлении автора: {ex.Message}");
            }
        }        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    return NotFound($"Автор с ID {id} не найден");
                }                
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при удалении автора: {ex.Message}");
            }
        }
        // LINQ-ЗАПРОСЫ       
        [HttpGet("with-book-count")]
        public async Task<ActionResult> GetAuthorsWithBookCount()
        {
            try
            {                
                var authorsWithCount = await _context.Authors
                    .Select(a => new
                    {
                        a.Id,
                        a.Name,
                        a.DateOfBirth,
                        BookCount = a.Books.Count
                    })
                    .ToListAsync();
                return Ok(authorsWithCount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при получении авторов: {ex.Message}");
            }
        }
        [HttpGet("search/{name}")]
        public async Task<ActionResult<IEnumerable<Author>>> SearchAuthors(string name)
        {
            try
            {              
                var authors = await _context.Authors
                    .Where(a => a.Name.Contains(name))
                    .Include(a => a.Books)
                    .ToListAsync();
                return Ok(authors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при поиске авторов: {ex.Message}");
            }
        }
        [HttpGet("search-start/{name}")]
        public async Task<ActionResult<IEnumerable<Author>>> SearchAuthorsStartsWith(string name)
        {
            try
            {             
                var authors = await _context.Authors
                    .Where(a => a.Name.StartsWith(name))
                    .Include(a => a.Books)
                    .ToListAsync();
                return Ok(authors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при поиске авторов: {ex.Message}");
            }
        }
        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
