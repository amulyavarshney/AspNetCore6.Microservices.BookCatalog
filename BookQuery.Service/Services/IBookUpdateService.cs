using BookQuery.Service.ViewModels;

namespace BookQuery.Service.Services
{
    public interface IBookUpdateService
    {
        Task UpdateAsync(MessageViewModel viewModel);
    }
}
