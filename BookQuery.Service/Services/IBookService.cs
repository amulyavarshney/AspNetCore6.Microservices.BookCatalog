using BookQuery.Service.Models;

namespace BookQuery.Service.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book> GetByIdAsync(int id);
    }
}
