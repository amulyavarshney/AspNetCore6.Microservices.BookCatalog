using BookQuery.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace BookQuery.Service.Context
{
    /* The BookUpdateContext class is a DbContext class that has a DbSet of Books and a connection
    string to the database */
    public class BookUpdateContext : DbContext
    {
        /* A constructor that takes in a configuration object and sets it to a private variable. */
        private readonly IConfiguration _configuration;
        public BookUpdateContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /* Creating a table in the database called Books. */
        public DbSet<Book> Books { get; set; }
        
        /// <summary>
        /// The `OnModelCreating` function is used to configure the database model
        /// </summary>
        /// <param name="ModelBuilder">The ModelBuilder instance to use.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().ToTable(nameof(Book));
        }

        /// <summary>
        /// The OnConfiguring method is called by the Entity Framework when the context is being created
        /// </summary>
        /// <param name="DbContextOptionsBuilder">This is the class that is used to configure the
        /// DbContext.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Development"));
        }
    }
}
