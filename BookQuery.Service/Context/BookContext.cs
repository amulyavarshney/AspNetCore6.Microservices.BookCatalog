using Microsoft.EntityFrameworkCore;
using BookQuery.Service.Models;

namespace BookQuery.Service.Context
{
    /* The BookContext class is a DbContext class that has a DbSet property for the Book class */
    public class BookContext : DbContext
    {
        /* This is the constructor for the BookContext class. It is calling the base class constructor
        and passing in the options parameter. */
        public BookContext(DbContextOptions<BookContext> options) : base(options) { }
        
        /* Creating a property called Books that is a DbSet of Book objects. */
        public DbSet<Book> Books { get; set; }

        /// <summary>
        /// The `OnModelCreating` function is used to configure the database model
        /// </summary>
        /// <param name="ModelBuilder">The ModelBuilder instance to use.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().ToTable(nameof(Book));
        }
    }
}
