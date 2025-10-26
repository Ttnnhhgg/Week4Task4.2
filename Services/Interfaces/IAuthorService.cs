using Course4.Models;
using Course4.Services;
namespace Course4.Services.Interfaces
{
    public interface IAuthorService
    {
        // Получить всех авторов
        Task<OperationResult<IEnumerable<Author>>> GetAllAuthorsAsync();
        // Получить автора по ID
        Task<OperationResult<Author>> GetAuthorByIdAsync(int id);
        // Создать нового автора
        Task<OperationResult<Author>> CreateAuthorAsync(Author author);
        // Обновить автора
        Task<OperationResult> UpdateAuthorAsync(int id, Author author);
        // Удалить автора
        Task<OperationResult> DeleteAuthorAsync(int id);
        // Поиск авторов по имени
        Task<OperationResult<IEnumerable<Author>>> SearchAuthorsAsync(string name);
        // Поиск авторов по началу имени
        Task<OperationResult<IEnumerable<Author>>> SearchAuthorsStartsWithAsync(string name);
        // Получить авторов с количеством книг
        Task<OperationResult<IEnumerable<object>>> GetAuthorsWithBookCountAsync();
    }
}