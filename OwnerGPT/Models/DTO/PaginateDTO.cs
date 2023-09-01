namespace OwnerGPT.Models.DTO
{
    public class PaginateDTO<T> where T : class
    {
        public int Page { get; set; }
        public int Total { get; set; }
        public int Pages { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
