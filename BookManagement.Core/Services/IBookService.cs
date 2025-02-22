using BookManagement.Core.ViewModels;

namespace BookManagement.Core.Services
{
    public interface IBookService
    {
        Task<int> AddBookAsync(BookViewForAddAndUpdateModel bookViewModel);
        Task<(int addedCount, List<string> skippedTitles)> AddBooksBulkAsync(IEnumerable<BookViewForAddAndUpdateModel> books);
        Task<IEnumerable<string>> GetBookTitlesAsync(int page, int pageSize);
        Task<BookViewModel> GetBookDetailsAsync(int id);
        Task UpdateBookAsync(int id, BookViewForAddAndUpdateModel bookViewModel);
        Task SoftDeleteBookAsync(int id);
        Task SoftDeleteBooksBulkAsync(IEnumerable<int> bookIds);
    }
}
