using OnlineBookStoreAPI.Models;

namespace OnlineBookStoreAPI.Services
{
    public interface IBooksRepository
    {
        Task<IEnumerable<Book>> SelectWithTitle(string title);
        Task<IEnumerable<Book>> SelectWithAuthor(string authorName);
    }
}
