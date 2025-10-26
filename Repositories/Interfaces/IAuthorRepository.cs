using Course4.Models;
namespace Course4.Repositories.Interfaces
{
    public interface IAuthorRepository
    {
        // Получить всех авторов с их книгами
        Task<IEnumerable<Author>> GetAllAsync();
        // Получить автора по ID с его книгами
        Task<Author?> GetByIdAsync(int id);
        // Получить автора по ID без книг (для проверки существования)
        Task<Author?> GetByIdWithoutBooksAsync(int id);
        // Создать нового автора
        Task<Author> CreateAsync(Author author);
        // Обновить автора
        Task UpdateAsync(Author author);
        // Удалить автора
        Task DeleteAsync(int id);
        // Проверить существование автора
        Task<bool> ExistsAsync(int id);
        // Поиск авторов по имени (Contains)
        Task<IEnumerable<Author>> SearchByNameAsync(string name);
        // Поиск авторов по началу имени (StartsWith)
        Task<IEnumerable<Author>> SearchByNameStartsWithAsync(string name);
        // Получить авторов с количеством книг
        Task<IEnumerable<object>> GetAuthorsWithBookCountAsync();
    }
}
