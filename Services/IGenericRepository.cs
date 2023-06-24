namespace OnlineBookStoreAPI.Services
{
    public interface IGenericRepository<T, IdType> where T : class
    {

        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(IdType id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T?> DeleteAsync(IdType id);
    }
}
