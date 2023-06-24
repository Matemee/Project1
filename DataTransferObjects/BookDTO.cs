namespace OnlineBookStoreAPI.DataTransferObjects
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime PublicationDate { get; set; }
        public IEnumerable<string> AuthorNames { get; set; }
        public IEnumerable<string> CategoryNames { get; set; }
    }
}
