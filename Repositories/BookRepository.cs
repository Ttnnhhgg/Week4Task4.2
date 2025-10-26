using Microsoft.EntityFrameworkCore;
using Course4.Data;
using Course4.Models;
using Course4.Repositories.Interfaces;
namespace Course4.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryContext _context;
        public BookRepository(LibraryContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books
                .Include(b => b.Author)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books
                .Include(b => b.Author)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<Book> CreateAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();           
            return await GetByIdAsync(book.Id) ?? book;
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Books
                .AsNoTracking()
                .AnyAsync(b => b.Id == id);
        }
        public async Task<IEnumerable<Book>> GetByAuthorIdAsync(int authorId)
        {
            return await _context.Books
                .Include(b => b.Author)
                .AsNoTracking()
                .Where(b => b.AuthorId == authorId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Book>> GetBooksPublishedAfterYearAsync(int year)
        {
            return await _context.Books
                .Include(b => b.Author)
                .AsNoTracking()
                .Where(b => b.PublishedYear > year)
                .OrderByDescending(b => b.PublishedYear)
                .ToListAsync();
        }
        public async Task<IEnumerable<Book>> SearchByTitleAsync(string title)
        {
            return await _context.Books
                .Include(b => b.Author)
                .AsNoTracking()
                .Where(b => b.Title.Contains(title))
                .ToListAsync();
        }
        public async Task<IEnumerable<Book>> SearchByTitleStartsWithAsync(string title)
        {
            return await _context.Books
                .Include(b => b.Author)
                .AsNoTracking()
                .Where(b => b.Title.StartsWith(title))
                .ToListAsync();
        }
        public async Task<int> GetBookCountByAuthorAsync(int authorId)
        {
            return await _context.Books
                .AsNoTracking()
                .CountAsync(b => b.AuthorId == authorId);
        }
    }
}