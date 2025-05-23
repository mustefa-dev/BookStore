namespace BookStore.DATA.DTOs.Book
{
    public class BookDto : BaseDto<Guid>
    {
        public string? Name { get; set; }
        public string? Author { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime PublishedDate { get; set; }
        public decimal Price { get; set; }
        public string? Genre { get; set; }
        public Guid CategoryId { get; set; }
        public int ? Stock { get; set; }
    }
}