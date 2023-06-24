namespace OnlineBookStoreAPI.DataTransferObjects
{
    public class CreateBookDTO
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Price { get; set; }
        public DateTime PublicationDate { get; set; }
        public required List<string> CategoryNames { get; set; }
        public required List<int> AuthorIds { get; set; }
    }
}
