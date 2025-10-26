using Microsoft.AspNetCore.Mvc;
using Course4.Models;
using Course4.Services.Interfaces;
namespace Course4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            var result = await _authorService.GetAllAuthorsAsync();
            return result.IsSuccess
                ? Ok(result.Data)
                : StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }        
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var result = await _authorService.GetAuthorByIdAsync(id);
            return result.IsSuccess
                ? Ok(result.Data)
                : StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }    
        [HttpPost]
        public async Task<ActionResult<Author>> CreateAuthor([FromBody] Author author)
        {           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authorService.CreateAuthorAsync(author);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetAuthor), new { id = result.Data!.Id }, result.Data);
            }
            return StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }       
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] Author author)
        {            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authorService.UpdateAuthorAsync(id, author);
            return result.IsSuccess
                ? NoContent()
                : StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var result = await _authorService.DeleteAuthorAsync(id);
            return result.IsSuccess
                ? NoContent()
                : StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }
        // LINQ-ЗАПРОСЫ       
        [HttpGet("with-book-count")]
        public async Task<ActionResult> GetAuthorsWithBookCount()
        {
            var result = await _authorService.GetAuthorsWithBookCountAsync();
            return result.IsSuccess
                ? Ok(result.Data)
                : StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }       
        [HttpGet("search/{name}")]
        public async Task<ActionResult<IEnumerable<Author>>> SearchAuthors(string name)
        {
            var result = await _authorService.SearchAuthorsAsync(name);
            return result.IsSuccess
                ? Ok(result.Data)
                : StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }       
        [HttpGet("search-start/{name}")]
        public async Task<ActionResult<IEnumerable<Author>>> SearchAuthorsStartsWith(string name)
        {
            var result = await _authorService.SearchAuthorsStartsWithAsync(name);
            return result.IsSuccess
                ? Ok(result.Data)
                : StatusCode(result.StatusCode, new { error = result.ErrorMessage });
        }
    }
}