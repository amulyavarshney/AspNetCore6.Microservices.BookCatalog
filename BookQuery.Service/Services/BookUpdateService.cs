using BookQuery.Service.Context;
using BookQuery.Service.Models;
using BookQuery.Service.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BookQuery.Service.Services
{
    /* It's a service class that implements the IBookUpdateService interface */
    public class BookUpdateService : IBookUpdateService
    {
        /* It's a constructor that takes a BookUpdateContext as a parameter and assigns it to the
        private readonly field _context. */
        private readonly BookUpdateContext _context;
        public BookUpdateService(BookUpdateContext context)
        {
            _context = context;
        }

        /// <summary>
        /// It takes a message, and based on the command, it either creates, updates, or deletes a
        /// record
        /// </summary>
        /// <param name="MessageViewModel">This is the object that is passed to the method. It contains
        /// the data that is needed to create, update, or delete the record.</param>
        /// <returns>
        /// The return type is Task.
        /// </returns>
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

        /// <summary>
        /// It creates a new book from the message view model.
        /// </summary>
        /// <param name="MessageViewModel"></param>
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

        /// <summary>
        /// It takes a message from the queue, and updates the database with the new information
        /// </summary>
        /// <param name="MessageViewModel"></param>
        private async Task UpdateFromMessageAsync(MessageViewModel viewModel)
        {
            var book = await FromId(viewModel.BookId);
            book.Description = viewModel.Description;
            book.Author = viewModel.Author;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// It deletes a book from the database using the book's id
        /// </summary>
        /// <param name="MessageViewModel"></param>
        private async Task DeleteFromMessageAsync(MessageViewModel viewModel)
        {
            var book = await FromId(viewModel.BookId);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// It takes an id, finds the book with that id, and returns it
        /// </summary>
        /// <param name="id">The id of the book to be deleted.</param>
        /// <returns>
        /// The book with the id that was passed in. If book is null, it throws an exception.
        /// </returns>
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
