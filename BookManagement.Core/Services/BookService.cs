using BookManagement.Core.ViewModels;
using BookManagement.Infrastructure.Entities;
using BookManagement.Infrastructure.Repositories;

namespace BookManagement.Core.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<int> AddBookAsync(BookViewModel bookViewModel)
        {
            var existingBook = await _bookRepository.GetByTitleAsync(bookViewModel.Title);
            if (existingBook != null)
                throw new Exception("A book with this title already exists.");

            var book = (BookEntity)bookViewModel;

            int bookId = await _bookRepository.AddAsync(book);

            return bookId;
        }

        public async Task<(int addedCount, List<string> skippedTitles)> AddBooksBulkAsync(IEnumerable<BookViewModel> books)
        {
            if (books == null || !books.Any())
                throw new ArgumentException("The book list cannot be empty.");


            var existingTitles = (await _bookRepository
                        .GetBookTitlesAsync()).ToHashSet();

            var newBooks = new List<BookEntity>();
            var skippedTitles = new List<string>();

            foreach (var bookViewModel in books)
            {
                if (existingTitles.Contains(bookViewModel.Title))
                {
                    skippedTitles.Add(bookViewModel.Title); // Track duplicates
                }
                else
                {
                    var book = (BookEntity)bookViewModel;
                    newBooks.Add(book);
                }
            }

            if (newBooks.Any())
            {
                await _bookRepository.AddBulkAsync(newBooks);
            }

            return (newBooks.Count, skippedTitles);
        }

        public async Task<IEnumerable<string>> GetBookTitlesAsync(int page, int pageSize)
        {
            var titles = await _bookRepository.GetBookTitlesAsync(page, pageSize);

            return titles;
        }

        public async Task<BookViewModel> GetBookDetailsAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null || book.IsDeleted)
                throw new Exception("Book not found or has been deleted.");

            book.ViewsCount++;
            await _bookRepository.UpdateAsync(book);

            return (BookViewModel)book;
        }

        public async Task UpdateBookAsync(int id, BookViewModel bookViewModel)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null || book.IsDeleted)
                throw new Exception("Book not found or has been deleted.");


            book.Title = bookViewModel.Title;
            book.PublicationYear = bookViewModel.PublicationYear;
            book.AuthorName = bookViewModel.AuthorName;

            await _bookRepository.UpdateAsync(book);
        }

        public async Task SoftDeleteBookAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
                throw new Exception("Book not found.");

            book.IsDeleted = true;
            await _bookRepository.UpdateAsync(book);
        }

        public async Task SoftDeleteBooksBulkAsync(IEnumerable<int> bookIds)
        {
            if (bookIds == null || !bookIds.Any())
                throw new ArgumentException("The book list cannot be empty.");

            await _bookRepository.DeleteBulkAsync(bookIds);
        }
    }

}
