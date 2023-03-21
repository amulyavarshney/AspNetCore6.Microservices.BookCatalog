using BookQuery.Service.ViewModels;

namespace BookQuery.Service.Services
{
    /* A method that is used to update the book. */
    public interface IBookUpdateService
    {
        Task UpdateAsync(MessageViewModel viewModel);
    }
}
