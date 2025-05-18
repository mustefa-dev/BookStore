using System.ComponentModel.DataAnnotations;

namespace BookStore.DATA.DTOs.Cart
{
    public class CartForm 
    {
        public List<OrderItemtForm> Items { get; set; } = new();
    }

    public class OrderItemtForm
    {
        [Required]
        public Guid BookId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; } = 1;
    }
}
