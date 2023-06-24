using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineBookStoreAPI.Models;
using OnlineBookStoreAPI.Services;
using OnlineBookStoreAPI.Data;

namespace OnlineBookStoreAPI.Repositories
{
    public class AuthorsRepository : IGenericRepository<Author, int>
    {
        private readonly AppDbContext _dbContext;


        public AuthorsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Author> GetByIdAsync(int id)
        {
            return await _dbContext.Authors
                    .Include(au => au.Books)
                    .FirstOrDefaultAsync(au => au.Id == id);
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _dbContext.Authors
                    .Include(au => au.Books)
                    .ToListAsync();
        }

        public async Task<Author> AddAsync(Author entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Author> UpdateAsync(Author entity)
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Author> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);

            if (entity != null)
            {
                _dbContext.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }

            return entity;
        }
    }
}