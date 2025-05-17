namespace BookStore.DATA.DTOs.Book
{
    public class BookForm
    {
        public string Name { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime PublishedDate { get; set; }
        public decimal Price { get; set; }
        public string Genre { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
    }
}