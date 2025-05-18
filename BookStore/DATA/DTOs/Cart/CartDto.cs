using BookStore.DATA.DTOs.CartProduct;

namespace BookStore.DATA.DTOs.Cart
{
    public class CartDto : BaseDto<Guid>
    {
        public Guid? UserId { get; set; }
        public List<CartProductDto> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
    }
}