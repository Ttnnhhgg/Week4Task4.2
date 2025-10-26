using Course4.Models;
using Course4.Repositories.Interfaces;
using Course4.Services.Interfaces;
namespace Course4.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        public AuthorService(IAuthorRepository authorRepository, IBookRepository bookRepository)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
        }
        public async Task<OperationResult<IEnumerable<Author>>> GetAllAuthorsAsync()
        {
            try
            {
                var authors = await _authorRepository.GetAllAsync();
                return OperationResult<IEnumerable<Author>>.Success(authors);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<Author>>.ServerError($"Ошибка при получении авторов: {ex.Message}");
            }
        }
        public async Task<OperationResult<Author>> GetAuthorByIdAsync(int id)
        {
            try
            {
                var author = await _authorRepository.GetByIdAsync(id);

                if (author == null)
                {
                    return OperationResult<Author>.NotFound($"Автор с ID {id} не найден");
                }
                return OperationResult<Author>.Success(author);
            }
            catch (Exception ex)
            {
                return OperationResult<Author>.ServerError($"Ошибка при получении автора: {ex.Message}");
            }
        }
        public async Task<OperationResult<Author>> CreateAuthorAsync(Author author)
        {
            try
            {              
                var validationResult = ValidateAuthor(author);
                if (!validationResult.IsValid)
                {
                    return OperationResult<Author>.ValidationError(validationResult.ErrorMessage!);
                }                
                var existingAuthors = await _authorRepository.SearchByNameAsync(author.Name);
                if (existingAuthors.Any(a => a.Name.ToLower() == author.Name.ToLower()))
                {
                    return OperationResult<Author>.ValidationError($"Автор с именем '{author.Name}' уже существует");
                }
                var createdAuthor = await _authorRepository.CreateAsync(author);
                return OperationResult<Author>.Success(createdAuthor);
            }
            catch (Exception ex)
            {
                return OperationResult<Author>.ServerError($"Ошибка при создании автора: {ex.Message}");
            }
        }
        public async Task<OperationResult> UpdateAuthorAsync(int id, Author author)
        {
            try
            {
                if (id != author.Id)
                {
                    return OperationResult.ValidationError("ID в URL не совпадает с ID в теле запроса");
                }                
                var validationResult = ValidateAuthor(author);
                if (!validationResult.IsValid)
                {
                    return OperationResult.ValidationError(validationResult.ErrorMessage!);
                }                
                var existingAuthor = await _authorRepository.GetByIdWithoutBooksAsync(id);
                if (existingAuthor == null)
                {
                    return OperationResult.NotFound($"Автор с ID {id} не найден");
                }
                await _authorRepository.UpdateAsync(author);
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.ServerError($"Ошибка при обновлении автора: {ex.Message}");
            }
        }
        public async Task<OperationResult> DeleteAuthorAsync(int id)
        {
            try
            {                
                var author = await _authorRepository.GetByIdWithoutBooksAsync(id);
                if (author == null)
                {
                    return OperationResult.NotFound($"Автор с ID {id} не найден");
                }               
                var bookCount = await _bookRepository.GetBookCountByAuthorAsync(id);
                if (bookCount > 0)
                {
                    return OperationResult.ValidationError("Нельзя удалить автора, у которого есть книги. Сначала удалите все книги автора.");
                }
                await _authorRepository.DeleteAsync(id);
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.ServerError($"Ошибка при удалении автора: {ex.Message}");
            }
        }
        public async Task<OperationResult<IEnumerable<Author>>> SearchAuthorsAsync(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return OperationResult<IEnumerable<Author>>.ValidationError("Поисковый запрос не может быть пустым");
                }
                var authors = await _authorRepository.SearchByNameAsync(name);
                return OperationResult<IEnumerable<Author>>.Success(authors);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<Author>>.ServerError($"Ошибка при поиске авторов: {ex.Message}");
            }
        }
        public async Task<OperationResult<IEnumerable<Author>>> SearchAuthorsStartsWithAsync(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return OperationResult<IEnumerable<Author>>.ValidationError("Поисковый запрос не может быть пустым");
                }
                var authors = await _authorRepository.SearchByNameStartsWithAsync(name);
                return OperationResult<IEnumerable<Author>>.Success(authors);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<Author>>.ServerError($"Ошибка при поиске авторов: {ex.Message}");
            }
        }
        public async Task<OperationResult<IEnumerable<object>>> GetAuthorsWithBookCountAsync()
        {
            try
            {
                var authorsWithCount = await _authorRepository.GetAuthorsWithBookCountAsync();
                return OperationResult<IEnumerable<object>>.Success(authorsWithCount);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<object>>.ServerError($"Ошибка при получении авторов: {ex.Message}");
            }
        }       
        private OperationResult ValidateAuthor(Author author)
        {
            if (string.IsNullOrWhiteSpace(author.Name))
            {
                return OperationResult.ValidationError("Имя автора обязательно для заполнения");
            }
            if (author.Name.Length > 100)
            {
                return OperationResult.ValidationError("Имя автора не может превышать 100 символов");
            }
            if (author.DateOfBirth > DateTime.Now)
            {
                return OperationResult.ValidationError("Дата рождения не может быть в будущем");
            }
            if (author.DateOfBirth < new DateTime(1000, 1, 1))
            {
                return OperationResult.ValidationError("Некорректная дата рождения");
            }
            return OperationResult.Success();
        }
    }
}