using BookManagement.Core.ViewModels;

namespace BookManagement.Core.Services
{
    public interface IBookService
    {
        Task<int> AddBookAsync(BookViewModel bookViewModel);
        Task<(int addedCount, List<string> skippedTitles)> AddBooksBulkAsync(IEnumerable<BookViewModel> books);
        Task<IEnumerable<string>> GetBookTitlesAsync(int page, int pageSize);
        Task<BookViewModel> GetBookDetailsAsync(int id);
        Task UpdateBookAsync(int id, BookViewModel bookViewModel);
        Task SoftDeleteBookAsync(int id);
        Task SoftDeleteBooksBulkAsync(IEnumerable<int> bookIds);
    }
}
