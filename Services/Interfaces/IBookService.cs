using Course4.Models;
using Course4.Services;
namespace Course4.Services.Interfaces
{
    public interface IBookService
    {
        // Получить все книги
        Task<OperationResult<IEnumerable<Book>>> GetAllBooksAsync();
        // Получить книгу по ID
        Task<OperationResult<Book>> GetBookByIdAsync(int id);
        // Создать новую книгу
        Task<OperationResult<Book>> CreateBookAsync(Book book);
        // Обновить книгу
        Task<OperationResult> UpdateBookAsync(int id, Book book);
        // Удалить книгу
        Task<OperationResult> DeleteBookAsync(int id);
        // Получить книги по автору
        Task<OperationResult<IEnumerable<Book>>> GetBooksByAuthorAsync(int authorId);
        // Получить книги, опубликованные после указанного года
        Task<OperationResult<IEnumerable<Book>>> GetBooksPublishedAfterYearAsync(int year);
        // Поиск книг по названию
        Task<OperationResult<IEnumerable<Book>>> SearchBooksAsync(string title);
        // Поиск книг по началу названия
        Task<OperationResult<IEnumerable<Book>>> SearchBooksStartsWithAsync(string title);
    }
}