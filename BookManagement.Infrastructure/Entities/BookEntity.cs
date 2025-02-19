namespace BookManagement.Infrastructure.Entities
{
    public class BookEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublicationYear { get; set; }
        public string AuthorName { get; set; }
        public int ViewsCount { get; set; }
        public bool IsDeleted { get; set; }
    }
}
