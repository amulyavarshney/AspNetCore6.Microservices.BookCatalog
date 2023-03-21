using BookCommand.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace BookCommand.Service.Context
{
    /* The `BookContext` class inherits from `DbContext` and overrides the `SaveChanges` and
    `SaveChangesAsync` methods to set the `IsDeleted` property to `true` when a record is deleted */
    public class BookContext : DbContext
    {
        /*It is calling the base class constructor and passing in the `DbContextOptions` object. */
        public BookContext(DbContextOptions<BookContext> options) : base(options) { }
        
        /* Creating a property called `Books` that is of type `DbSet<Book>`. */
        public DbSet<Book> Books { get; set; }

        /// <summary>
        /// "When you query for a Book, only return the ones that have IsDeleted set to false."
        /// 
        /// This is a great way to implement soft deletes in your database
        /// </summary>
        /// <param name="ModelBuilder">The ModelBuilder instance to use.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>().ToTable(nameof(Book));

            modelBuilder.Entity<Book>().Property<bool>("IsDeleted");
            modelBuilder.Entity<Book>().HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);
        }

        /// <summary>
        /// It will check if any of the entities in the context are marked as deleted, and if so, it
        /// will set their IsDeleted property to true before saving changes to the database
        /// </summary>
        /// <returns>
        /// The number of objects written to the underlying database.
        /// </returns>
        public override int SaveChanges()
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChanges();
        }

        /// <summary>
        /// > If the entity is marked for deletion, then set the IsDeleted property to true
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">A Boolean value that indicates whether Entity
        /// Framework should automatically call AcceptAllChanges after it saves changes
        /// successfully.</param>
        /// <param name="CancellationToken">A cancellation token that can be used by other objects or
        /// threads to receive notice of cancellation.</param>
        /// <returns>
        /// The base.SaveChangesAsync method is being returned.
        /// </returns>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// > If an entity is being added, set the IsDeleted property to false. If an entity is being
        /// deleted, set the IsDeleted property to true
        /// </summary>
        private void UpdateSoftDeleteStatuses()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["IsDeleted"] = false;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                        break;
                }
            }
        }
    }
}
