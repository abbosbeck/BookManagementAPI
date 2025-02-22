using BookManagement.Infrastructure.Entities;

namespace BookManagement.Core.ViewModels
{
    public class BookViewForAddAndUpdateModel
    {
        public string Title { get; set; }
        public DateTime PublicationYear { get; set; }
        public string AuthorName { get; set; }

        public static explicit operator BookEntity(BookViewForAddAndUpdateModel BookViewForAddAndUpdateModel)
        {
            return new BookEntity
            {
                Title = BookViewForAddAndUpdateModel.Title,
                PublicationYear = BookViewForAddAndUpdateModel.PublicationYear,
                AuthorName = BookViewForAddAndUpdateModel.AuthorName
            };
        }
    }
}
