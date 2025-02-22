using BookManagement.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<BookEntity> Books { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<ISoftDeletable>()
                .Where(e => e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                entry.Entity.IsDeleted = true;
                entry.State = EntityState.Modified;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookEntity>()
                .Property(b => b.ViewsCount)
                .HasDefaultValue(0);

            modelBuilder.Entity<BookEntity>()
                .Property(b => b.IsDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<BookEntity>()
                .HasQueryFilter(modelBuilder => !modelBuilder.IsDeleted);
        }
    }
}
