using BookManagement.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> GetBookTitlesAsync()
        {
            return await _context.Books
                .Select(b => b.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetBookTitlesAsync(int page, int pageSize)
        {
            return await _context.Books
                .OrderByDescending(b => b.ViewsCount * 0.5 + (DateTime.Now.Year - b.PublicationYear.Year) * 2)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => b.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookEntity>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.Books
                .Where(b => ids.Contains(b.Id))
                .ToListAsync();
        }

        public async Task<BookEntity> GetByTitleAsync(string title)
        {
            return await _context.Books
           .Where(b => b.Title == title)
           .FirstOrDefaultAsync();
        }

        public async Task<BookEntity> GetByIdAsync(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<int> AddAsync(BookEntity book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return book.Id;
        }

        public async Task AddBulkAsync(IEnumerable<BookEntity> books)
        {
            await _context.Books.AddRangeAsync(books);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BookEntity book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteBulkAsync(IEnumerable<int> bookIds)
        {
            await _context.Books
                .Where(b => bookIds.Contains(b.Id))
                .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.IsDeleted, true));
        }
    }
}
