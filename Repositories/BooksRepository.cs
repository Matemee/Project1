using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineBookStoreAPI.Models;
using OnlineBookStoreAPI.Services;
using OnlineBookStoreAPI.Data;


namespace OnlineBookStoreAPI.Repositories
{
    public class BooksRepository : IGenericRepository<Book, int>, IBooksRepository
    {
        private readonly AppDbContext _dbContext;

        public BooksRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
      

        public async Task<Book> GetByIdAsync(int id)
        {
            return await _dbContext.Books.FindAsync(id);
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _dbContext.Books
              .Include(bo => bo.Authors)
              .Include(bo => bo.Categories)
              .ToListAsync();
        }

        public async Task<Book> AddAsync(Book entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Book> UpdateAsync(Book entity)
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Book?> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);

            if (entity != null)
            {
                _dbContext.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }

            return entity;
        }
        

        public async Task<IEnumerable<Book>> SelectWithAuthor(string authorName)
        {
            return await _dbContext.Books
                .Include(b => b.Authors)
                .Where(b => b.Authors.Any(a => a.Name == authorName))
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> SelectWithTitle(string title)
        {
            return await _dbContext.Books
                .Where(b => b.Title.StartsWith(title))
                .ToListAsync();
        }
    }
}