using BookManagement.Core.ViewModels;
using BookManagement.Infrastructure;
using BookManagement.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Core.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly AppDbContext _context;

        public BookService(IBookRepository bookRepository, AppDbContext context)
        {
            _bookRepository = bookRepository;
            _context = context;
        }

        public async Task<int> AddBookAsync(BookViewModel bookViewModel)
        {
            // Validation logic

            var existingBook = _context.Books
                .Where(b => b.Title == bookViewModel.Title);

            if (existingBook == null)
                throw new Exception("A book with this title already exists.");

            var book = (BookEntity)bookViewModel;

            var entry = await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return entry.Entity.Id;
        }

        public async Task<(int addedCount, List<string> skippedTitles)> AddBooksBulkAsync(IEnumerable<BookViewModel> books)
        {
            if (books == null || !books.Any())
                throw new ArgumentException("The book list cannot be empty.");


            var existingTitles =
                (await _context.Books
                    .Select(b => b.Title)
                    .ToListAsync()).ToHashSet();

            var newBooks = new List<BookEntity>();
            var skippedTitles = new List<string>();

            foreach (var bookViewModel in books)
            {
                if (existingTitles.Contains(bookViewModel.Title))
                {
                    skippedTitles.Add(bookViewModel.Title);
                }
                else
                {
                    var book = (BookEntity)bookViewModel;
                    newBooks.Add(book);
                }
            }

            if (newBooks.Any())
            {
                await _context.Books.AddRangeAsync((newBooks));
                await _context.SaveChangesAsync();
            }

            return (newBooks.Count, skippedTitles);
        }

        public async Task<IEnumerable<string>> GetBookTitlesAsync(int page, int pageSize)
        {
            // Validation logic 

            var titles = await _context.Books
                            .OrderByDescending(b => b.ViewsCount * 0.5 + (DateTime.Now.Year - b.PublicationYear.Year) * 2)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Select(b => b.Title)
                            .ToListAsync();

            return titles;
        }

        public async Task<BookViewModel> GetBookDetailsAsync(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book == null || book.IsDeleted)
                throw new Exception("Book not found or has been deleted.");

            book.ViewsCount++;
            var entry = _context.Books.Update(book);
            await _context.SaveChangesAsync();

            return (BookViewModel)entry.Entity;
        }

        public async Task UpdateBookAsync(int id, BookViewModel bookViewModel)
        {
            var book = await _context.Books
                    .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null || book.IsDeleted)
                throw new Exception("Book not found or has been deleted.");

            book.Title = bookViewModel.Title;
            book.AuthorName = bookViewModel.AuthorName;
            book.PublicationYear = bookViewModel.PublicationYear;

            _context.Books.Update(book);
            await _context.SaveChangesAsync(); ;
        }

        public async Task SoftDeleteBookAsync(int id)
        {
            var book = await _context.Books
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                throw new Exception("Book not found.");

            book.IsDeleted = true;
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteBooksBulkAsync(IEnumerable<int> bookIds)
        {
            if (bookIds == null || !bookIds.Any())
                throw new ArgumentException("The book list cannot be empty.");

            await _context.Books
                .Where(b => bookIds.Contains(b.Id))
                .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.IsDeleted, true));
        }
    }

}
