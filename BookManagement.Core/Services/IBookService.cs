using BookManagement.Core.ViewModels;

namespace BookManagement.Core.Services
{
    public interface IBookService
    {
        Task AddBookAsync(BookViewModel bookViewModel);
        Task<(int AddedCount, List<string> SkippedTitles)> AddBooksBulkAsync(IEnumerable<BookViewModel> books);
        Task<IEnumerable<string>> GetBookTitlesAsync(int page, int pageSize);
        Task<BookViewModel> GetBookDetailsAsync(int id);
        Task UpdateBookAsync(int id, BookViewModel bookViewModel);
        Task SoftDeleteBookAsync(int id);
        Task SoftDeleteBooksBulkAsync(IEnumerable<int> bookIds);
    }
}
