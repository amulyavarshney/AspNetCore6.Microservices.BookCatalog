/* Creating an interface for the BookService. */
using BookCommand.Service.ViewModels;

namespace BookCommand.Service.Services
{
    /* Creating an interface for the BookService. */
    public interface IBookService
    {
        Task CreateAsync(BookCreateViewModel book);
        Task UpdateAsync(int id, BookUpdateViewModel book);
        Task DeleteAsync(int id);
    }
}
