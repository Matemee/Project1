using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineBookStoreAPI.Models;
using OnlineBookStoreAPI.Services;
using OnlineBookStoreAPI.Data;

namespace OnlineBookStoreAPI.Repositories
{
    public class CategoriesRepository : IGenericRepository<Category, string>
    {
        private readonly AppDbContext _dbContext;

        public CategoriesRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Category?> GetByIdAsync(string id)
        {
            return await _dbContext.Categories
                        .Include(ct => ct.Books)
                        .FirstOrDefaultAsync(ct => ct.Name == id);
        }


        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dbContext.Categories
                    .Include(ct => ct.Books)
                    .ToListAsync();
        }

        public async Task<Category> AddAsync(Category entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Category> UpdateAsync(Category entity)
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Category?> DeleteAsync(string id)
        {
            var targetCategory = await GetByIdAsync(id);
            if (targetCategory == null)
                return null;
            _dbContext.Categories.Remove(targetCategory);
            await _dbContext.SaveChangesAsync();

            return targetCategory;
        }
    }
}
