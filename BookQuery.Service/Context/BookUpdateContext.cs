using BookQuery.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace BookQuery.Service.Context
{
    public class BookUpdateContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public BookUpdateContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<Book> Books { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().ToTable(nameof(Book));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Development"));
        }
    }
}
