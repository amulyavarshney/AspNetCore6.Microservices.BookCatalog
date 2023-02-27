using BookQuery.Service.Context;
using BookQuery.Service.Models;
using BookQuery.Service.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BookQuery.Service.Services
{
    public class BookUpdateService : IBookUpdateService
    {
        private readonly BookUpdateContext _context;
        public BookUpdateService(BookUpdateContext context)
        {
            _context = context;
        }

        public async Task UpdateAsync(MessageViewModel viewModel)
        {
            switch (viewModel.Command)
            {
                case Command.Create:
                    {
                        await CreateFromMessageAsync(viewModel);
                        return;
                    }
                case Command.Update:
                    {
                        await UpdateFromMessageAsync(viewModel);
                        return;
                    }
                case Command.Delete:
                    {
                        await DeleteFromMessageAsync(viewModel);
                        return;
                    }
                default:
                    {
                        return;
                    }
            }
        }

        private async Task CreateFromMessageAsync(MessageViewModel viewModel)
        {
            var book = new Book
            {
                Id = viewModel.BookId,
                Title = viewModel.Title,
                Description = viewModel.Description,
                Author = viewModel.Author,
            };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        private async Task UpdateFromMessageAsync(MessageViewModel viewModel)
        {
            var book = await FromId(viewModel.BookId);
            book.Description = viewModel.Description;
            book.Author = viewModel.Author;
            await _context.SaveChangesAsync();
        }

        private async Task DeleteFromMessageAsync(MessageViewModel viewModel)
        {
            var book = await FromId(viewModel.BookId);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        private async Task<Book> FromId(int id)
        {
            var book = await _context.Books
                .FirstOrDefaultAsync(c => c.Id == id);
            if (book is null)
            {
                throw new Exception($"Could not find the Book with id: {id}");
            }
            return book;
        }
    }
}
