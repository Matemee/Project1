namespace OnlineBookStoreAPI.DataTransferObjects
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public IEnumerable<int> BookIds { get; set; }
        public IEnumerable<string> BookTitles { get; set; }
        public IEnumerable<string> BookPublishDates { get; set; }
    }
}
