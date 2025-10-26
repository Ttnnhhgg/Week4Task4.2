using Microsoft.EntityFrameworkCore;
using Course4.Data;
using Course4.Models;
using Course4.Repositories.Interfaces;
namespace Course4.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryContext _context;
        public AuthorRepository(LibraryContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _context.Authors
                .Include(a => a.Books)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Author?> GetByIdAsync(int id)
        {
            return await _context.Authors
                .Include(a => a.Books)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<Author?> GetByIdWithoutBooksAsync(int id)
        {
            return await _context.Authors
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<Author> CreateAsync(Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return author;
        }
        public async Task UpdateAsync(Author author)
        {
            _context.Entry(author).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Authors
                .AsNoTracking()
                .AnyAsync(a => a.Id == id);
        }
        public async Task<IEnumerable<Author>> SearchByNameAsync(string name)
        {
            return await _context.Authors
                .Include(a => a.Books)
                .AsNoTracking()
                .Where(a => a.Name.Contains(name))
                .ToListAsync();
        }
        public async Task<IEnumerable<Author>> SearchByNameStartsWithAsync(string name)
        {
            return await _context.Authors
                .Include(a => a.Books)
                .AsNoTracking()
                .Where(a => a.Name.StartsWith(name))
                .ToListAsync();
        }
        public async Task<IEnumerable<object>> GetAuthorsWithBookCountAsync()
        {
            return await _context.Authors
                .Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.DateOfBirth,
                    BookCount = a.Books.Count
                })
                .AsNoTracking()
                .ToListAsync();
        }
    }
}