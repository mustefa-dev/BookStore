namespace BookStore.DATA.DTOs.Book
{
    public class BookFilter : BaseFilter
    {
        public string? Name { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public DateTime? PublishedDateFrom { get; set; }
        public DateTime? PublishedDateTo { get; set; }
        public Guid? CategoryId { get; set; }
    }
}