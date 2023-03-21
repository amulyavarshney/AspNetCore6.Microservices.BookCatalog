using BookQuery.Service.Context;
using BookQuery.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace BookQuery.Service.Services
{
    /* The BookService class is a service class that implements the IBookService interface */
    public class BookService : IBookService
    {
        /* Creating a private variable called _context and then creating a public constructor that
        takes in a BookContext object and sets the private variable to the BookContext object. */
        private readonly BookContext _context;
        public BookService(BookContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// > This function returns a list of books from the database
        /// </summary>
        /// <returns>
        /// A list of books
        /// </returns>
        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books.ToListAsync();
        }

        /// <summary>
        /// It returns a book with the given id, or throws an exception if the book is not found
        /// </summary>
        /// <param name="id">The id of the book to be retrieved.</param>
        /// <returns>
        /// The book with the id that was passed in.
        /// </returns>
        public async Task<Book> GetByIdAsync(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(c => c.Id == id);
            if (book == null)
            {
                throw new Exception($"Could not find the Book with id: {id}");
            }
            return book;
        }
    }
}
