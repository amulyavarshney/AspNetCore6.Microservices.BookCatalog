using BookCommand.Service.ViewModels;

namespace BookCommand.Service.Services
{
    public interface IBookService
    {
        Task CreateAsync(BookCreateViewModel book);
        Task UpdateAsync(int id, BookUpdateViewModel book);
        Task DeleteAsync(int id);
    }
}
