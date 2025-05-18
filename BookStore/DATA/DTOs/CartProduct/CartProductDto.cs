using BookStore.DATA.DTOs.Book;

namespace BookStore.DATA.DTOs.CartProduct
{
    public class CartProductDto : BaseDto<Guid>
    {
        public Guid BookId { get; set; }
        public BookDto? Book { get; set; }
        public int Quantity { get; set; }
    }
}