using Course4.Models;
namespace Course4.Repositories.Interfaces
{
    public interface IBookRepository
    {
        // Получить все книги с информацией об авторах
        Task<IEnumerable<Book>> GetAllAsync();
        // Получить книгу по ID с информацией об авторе
        Task<Book?> GetByIdAsync(int id);
        // Создать новую книгу
        Task<Book> CreateAsync(Book book);
        // Обновить книгу
        Task UpdateAsync(Book book);
        // Удалить книгу
        Task DeleteAsync(int id);
        // Проверить существование книги
        Task<bool> ExistsAsync(int id);
        // Получить книги по автору
        Task<IEnumerable<Book>> GetByAuthorIdAsync(int authorId);
        // Получить книги, опубликованные после указанного года
        Task<IEnumerable<Book>> GetBooksPublishedAfterYearAsync(int year);
        // Поиск книг по названию (Contains)
        Task<IEnumerable<Book>> SearchByTitleAsync(string title);
        // Поиск книг по началу названия (StartsWith)
        Task<IEnumerable<Book>> SearchByTitleStartsWithAsync(string title);
        // Получить количество книг у автора
        Task<int> GetBookCountByAuthorAsync(int authorId);
    }
}