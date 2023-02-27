using BookCommand.Service.Context;
using BookCommand.Service.Models;
using BookCommand.Service.ViewModels;
using Microsoft.EntityFrameworkCore;
using BookCommand.Service.MQ;
using System.Data;

namespace BookCommand.Service.Services
{
    public class BookService : IBookService
    {
        private readonly BookContext _context;
        private readonly IRabbitMQManager _rabbitMQManager;
        //private readonly HttpClient _client;
        public BookService(BookContext context, IRabbitMQManager rabbitMQManager)
        {
            _context = context;
            _rabbitMQManager = rabbitMQManager;
        }

        public async Task CreateAsync(BookCreateViewModel book)
        {
            var bookEntity = ToEntity(book);
            await _context.Books.AddAsync(bookEntity);
            await _context.SaveChangesAsync();

            var message = ToMessageViewModel(bookEntity, Command.Create);
            _rabbitMQManager.Publish(message, "ms-exchange", "topic", "cqrs");
        }

        public async Task UpdateAsync(int id, BookUpdateViewModel book)
        {
            var bookEntity = await FromId(id);
            bookEntity.Description = book.Description;
            bookEntity.Author = book.Author;
            await _context.SaveChangesAsync();

            var message = ToMessageViewModel(bookEntity, Command.Update);
            _rabbitMQManager.Publish(message, "ms-exchange", "topic", "cqrs");
        }

        public async Task DeleteAsync(int id)
        {
            var bookEntity = await FromId(id);
            _context.Books.Remove(bookEntity);
            await _context.SaveChangesAsync();

            var message = new MessageViewModel
            {
                Command = Command.Delete,
                BookId = bookEntity.Id
            };
            _rabbitMQManager.Publish(message, "ms-exchange", "topic", "cqrs");
        }

        private MessageViewModel ToMessageViewModel(Book book, Command command)
        {
            return new MessageViewModel
            {
                Command = command,
                BookId = book.Id,
                Title = book.Title,
                Description = book.Description,
                Author = book.Author,
            };
        }

        private Book ToEntity(BookCreateViewModel book)
        {
            return new Book
            {
                Title = book.Title,
                Description = book.Description,
                Author = book.Author,
            };
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
