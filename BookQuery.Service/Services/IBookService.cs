using BookQuery.Service.Models;

namespace BookQuery.Service.Services
{
    /* Defining the interface for the BookService class. */
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book> GetByIdAsync(int id);
    }
}
