namespace BookManagement.Infrastructure.Entities
{
    interface ISoftDeletable
    {
        public bool IsDeleted { get; set; }
    }
}
