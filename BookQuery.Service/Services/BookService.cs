using BookQuery.Service.Context;
using BookQuery.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace BookQuery.Service.Services
{
    public class BookService : IBookService
    {
        private readonly BookContext _context;
        public BookService(BookContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books.ToListAsync();
        }
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
