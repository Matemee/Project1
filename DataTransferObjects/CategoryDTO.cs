namespace OnlineBookStoreAPI.DataTransferObjects
{
    public class CategoryDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> BookIds { get; set; }
    }
}
