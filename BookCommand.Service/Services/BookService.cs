using BookCommand.Service.Context;
using BookCommand.Service.Models;
using BookCommand.Service.ViewModels;
using Microsoft.EntityFrameworkCore;
using BookCommand.Service.MQ;
using System.Data;

namespace BookCommand.Service.Services
{
    /* The BookService class is responsible for creating, updating and deleting books */
    public class BookService : IBookService
    {
        private readonly BookContext _context;
        private readonly IRabbitMQManager _rabbitMQManager;
        
        /* This is the constructor for the BookService class. It is taking in two parameters, a
        BookContext object and a IRabbitMQManager object. It is then assigning those parameters to
        the private variables _context and _rabbitMQManager. */
        public BookService(BookContext context, IRabbitMQManager rabbitMQManager)
        {
            _context = context;
            _rabbitMQManager = rabbitMQManager;
        }

        /// <summary>
        /// We create a book, save it to the database, and then publish a message to RabbitMQ
        /// </summary>
        /// <param name="BookCreateViewModel">This is the model that is used to create a book.</param>
        public async Task CreateAsync(BookCreateViewModel book)
        {
            var bookEntity = ToEntity(book);
            await _context.Books.AddAsync(bookEntity);
            await _context.SaveChangesAsync();

            var message = ToMessageViewModel(bookEntity, Command.Create);
            _rabbitMQManager.Publish(message, "ms-exchange", "topic", "cqrs");
        }

        /// <summary>
        /// We get the book from the database, update it, save it, and then publish a message to
        /// RabbitMQ
        /// </summary>
        /// <param name="id">The id of the book to update</param>
        /// <param name="BookUpdateViewModel">This is the view model that is passed in from the
        /// controller.</param>
        public async Task UpdateAsync(int id, BookUpdateViewModel book)
        {
            var bookEntity = await FromId(id);
            bookEntity.Description = book.Description;
            bookEntity.Author = book.Author;
            await _context.SaveChangesAsync();

            var message = ToMessageViewModel(bookEntity, Command.Update);
            _rabbitMQManager.Publish(message, "ms-exchange", "topic", "cqrs");
        }

        /// <summary>
        /// We delete the book from the database and then publish a message to RabbitMQ
        /// </summary>
        /// <param name="id">The id of the book to be deleted</param>
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

        /// <summary>
        /// It takes a book and a command and returns a message view model
        /// </summary>
        /// <param name="Book">The book object that is being passed in.</param>
        /// <param name="Command">This is the command that was sent to the service bus.</param>
        /// <returns>
        /// A new instance of the MessageViewModel class.
        /// </returns>
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

        /// <summary>
        /// It takes a BookCreateViewModel object and returns a Book object.
        /// </summary>
        /// <param name="BookCreateViewModel">This is the model that is passed in from the view.</param>
        /// <returns>
        /// A new Book object with the properties Title, Description, and Author.
        /// </returns>
        private Book ToEntity(BookCreateViewModel book)
        {
            return new Book
            {
                Title = book.Title,
                Description = book.Description,
                Author = book.Author,
            };
        }

        /// <summary>
        /// It takes an id, finds the book with that id, and returns it
        /// </summary>
        /// <param name="id">The id of the book to be found.</param>
        /// <returns>
        /// The book with the id that was passed in. If it is null, it throws an exception.
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
