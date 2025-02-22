using BookManagement.Infrastructure.Entities;

namespace BookManagement.Core.ViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublicationYear { get; set; }
        public string AuthorName { get; set; }
        public int ViewCount { get; set; }

        public static explicit operator BookViewModel(BookEntity bookEntity)
        {
            return new BookViewModel
            {
                Id = bookEntity.Id,
                Title = bookEntity.Title,
                PublicationYear = bookEntity.PublicationYear,
                AuthorName = bookEntity.AuthorName,
                ViewCount = bookEntity.ViewsCount
            };
        }

        public static explicit operator BookEntity(BookViewModel bookViewModel)
        {
            return new BookEntity
            {
                Id = bookViewModel.Id,
                Title = bookViewModel.Title,
                PublicationYear = bookViewModel.PublicationYear,
                AuthorName = bookViewModel.AuthorName
            };
        }
    }
}
