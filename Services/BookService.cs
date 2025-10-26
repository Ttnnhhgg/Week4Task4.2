using Course4.Models;
using Course4.Repositories.Interfaces;
using Course4.Services.Interfaces;

namespace Course4.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }
        public async Task<OperationResult<IEnumerable<Book>>> GetAllBooksAsync()
        {
            try
            {
                var books = await _bookRepository.GetAllAsync();
                return OperationResult<IEnumerable<Book>>.Success(books);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<Book>>.ServerError($"Ошибка при получении книг: {ex.Message}");
            }
        }
        public async Task<OperationResult<Book>> GetBookByIdAsync(int id)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(id);
                if (book == null)
                {
                    return OperationResult<Book>.NotFound($"Книга с ID {id} не найдена");
                }
                return OperationResult<Book>.Success(book);
            }
            catch (Exception ex)
            {
                return OperationResult<Book>.ServerError($"Ошибка при получении книги: {ex.Message}");
            }
        }
        public async Task<OperationResult<Book>> CreateBookAsync(Book book)
        {
            try
            {                
                var validationResult = ValidateBook(book);
                if (!validationResult.IsValid)
                {
                    return OperationResult<Book>.ValidationError(validationResult.ErrorMessage!);
                }                
                var authorExists = await _authorRepository.ExistsAsync(book.AuthorId);
                if (!authorExists)
                {
                    return OperationResult<Book>.ValidationError("Автор с указанным ID не существует");
                }
                var createdBook = await _bookRepository.CreateAsync(book);
                return OperationResult<Book>.Success(createdBook);
            }
            catch (Exception ex)
            {
                return OperationResult<Book>.ServerError($"Ошибка при создании книги: {ex.Message}");
            }
        }
        public async Task<OperationResult> UpdateBookAsync(int id, Book book)
        {
            try
            {
                if (id != book.Id)
                {
                    return OperationResult.ValidationError("ID в URL не совпадает с ID в теле запроса");
                }                
                var validationResult = ValidateBook(book);
                if (!validationResult.IsValid)
                {
                    return OperationResult.ValidationError(validationResult.ErrorMessage!);
                }              
                var existingBook = await _bookRepository.GetByIdAsync(id);
                if (existingBook == null)
                {
                    return OperationResult.NotFound($"Книга с ID {id} не найдена");
                }                
                var authorExists = await _authorRepository.ExistsAsync(book.AuthorId);
                if (!authorExists)
                {
                    return OperationResult.ValidationError("Автор с указанным ID не существует");
                }
                await _bookRepository.UpdateAsync(book);
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.ServerError($"Ошибка при обновлении книги: {ex.Message}");
            }
        }
        public async Task<OperationResult> DeleteBookAsync(int id)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(id);
                if (book == null)
                {
                    return OperationResult.NotFound($"Книга с ID {id} не найдена");
                }
                await _bookRepository.DeleteAsync(id);
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.ServerError($"Ошибка при удалении книги: {ex.Message}");
            }
        }
        public async Task<OperationResult<IEnumerable<Book>>> GetBooksByAuthorAsync(int authorId)
        {
            try
            {              
                var authorExists = await _authorRepository.ExistsAsync(authorId);
                if (!authorExists)
                {
                    return OperationResult<IEnumerable<Book>>.NotFound($"Автор с ID {authorId} не найден");
                }
                var books = await _bookRepository.GetByAuthorIdAsync(authorId);
                return OperationResult<IEnumerable<Book>>.Success(books);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<Book>>.ServerError($"Ошибка при получении книг: {ex.Message}");
            }
        }
        public async Task<OperationResult<IEnumerable<Book>>> GetBooksPublishedAfterYearAsync(int year)
        {
            try
            {
                if (year < 1000 || year > DateTime.Now.Year)
                {
                    return OperationResult<IEnumerable<Book>>.ValidationError($"Год должен быть между 1000 и {DateTime.Now.Year}");
                }
                var books = await _bookRepository.GetBooksPublishedAfterYearAsync(year);
                return OperationResult<IEnumerable<Book>>.Success(books);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<Book>>.ServerError($"Ошибка при получении книг: {ex.Message}");
            }
        }
        public async Task<OperationResult<IEnumerable<Book>>> SearchBooksAsync(string title)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title))
                {
                    return OperationResult<IEnumerable<Book>>.ValidationError("Поисковый запрос не может быть пустым");
                }
                var books = await _bookRepository.SearchByTitleAsync(title);
                return OperationResult<IEnumerable<Book>>.Success(books);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<Book>>.ServerError($"Ошибка при поиске книг: {ex.Message}");
            }
        }
        public async Task<OperationResult<IEnumerable<Book>>> SearchBooksStartsWithAsync(string title)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title))
                {
                    return OperationResult<IEnumerable<Book>>.ValidationError("Поисковый запрос не может быть пустым");
                }
                var books = await _bookRepository.SearchByTitleStartsWithAsync(title);
                return OperationResult<IEnumerable<Book>>.Success(books);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<Book>>.ServerError($"Ошибка при поиске книг: {ex.Message}");
            }
        }      
        private OperationResult ValidateBook(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
            {
                return OperationResult.ValidationError("Название книги обязательно для заполнения");
            }
            if (book.Title.Length > 200)
            {
                return OperationResult.ValidationError("Название книги не может превышать 200 символов");
            }
            if (book.PublishedYear < 1000 || book.PublishedYear > DateTime.Now.Year)
            {
                return OperationResult.ValidationError($"Год публикации должен быть между 1000 и {DateTime.Now.Year}");
            }
            return OperationResult.Success();
        }
    }
}