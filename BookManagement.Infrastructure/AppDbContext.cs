using BookManagement.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<BookEntity> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookEntity>()
                .Property(b => b.ViewsCount)
                .HasDefaultValue(0);

            modelBuilder.Entity<BookEntity>()
                .Property(b => b.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
