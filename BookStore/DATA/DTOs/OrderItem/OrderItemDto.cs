namespace BookStore.DATA.DTOs
{
    public class OrderItemDto : BaseDto<Guid>
    {
        public string BookName { get; set; }
        public string BookAuthor { get; set; }
        public string BookDescription { get; set; }
        public string BookImageUrl { get; set; }
        public DateTime? BookPublishedDate { get; set; }
        public int BookPrice { get; set; }
        public string BookGenre { get; set; }
        public Guid BookCategoryId { get; set; }
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }
    }
}