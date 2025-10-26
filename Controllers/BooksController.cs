using Microsoft.AspNetCore.Mvc;
using Course4.Models;
using Course4.Services.Interfaces;
namespace Course4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var result = await _bookService.GetAllBooksAsync();
            return result.IsSuccess
                ? Ok(result.Data)
                : StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }     
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var result = await _bookService.GetBookByIdAsync(id);
            return result.IsSuccess
                ? Ok(result.Data)
                : StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }        
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook([FromBody] Book book)
        {            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _bookService.CreateBookAsync(book);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetBook), new { id = result.Data!.Id }, result.Data);
            }
            return StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }      
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book book)
        {            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _bookService.UpdateBookAsync(id, book);
            return result.IsSuccess
                ? NoContent()
                : StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _bookService.DeleteBookAsync(id);
            return result.IsSuccess
                ? NoContent()
                : StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }
        // LINQ-ЗАПРОСЫ       
        [HttpGet("byauthor/{authorId}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByAuthor(int authorId)
        {
            var result = await _bookService.GetBooksByAuthorAsync(authorId);
            return result.IsSuccess
                ? Ok(result.Data)
                : StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }      
        [HttpGet("recent")]
        public async Task<ActionResult<IEnumerable<Book>>> GetRecentBooks()
        {            
            var result = await _bookService.GetBooksPublishedAfterYearAsync(2015);
            return result.IsSuccess
                ? Ok(result.Data)
                : StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }        
        [HttpGet("search/{title}")]
       public async Task<ActionResult<IEnumerable<Book>>> SearchBooks(string title)
        {
            var result = await _bookService.SearchBooksAsync(title);
            return result.IsSuccess
                ? Ok(result.Data)
                : StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }       
        [HttpGet("search-start/{title}")]
        public async Task<ActionResult<IEnumerable<Book>>> SearchBooksStartsWith(string title)
        {
            var result = await _bookService.SearchBooksStartsWithAsync(title);
            return result.IsSuccess
                ? Ok(result.Data)
                : StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }
    }
}