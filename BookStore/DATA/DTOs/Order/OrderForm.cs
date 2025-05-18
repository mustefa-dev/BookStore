using System.ComponentModel.DataAnnotations;
using BookStore.DATA.DTOs.Cart;

namespace BookStore.DATA.DTOs.Order
{

    public class OrderForm 
    {
        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public List <OrderItemtForm> OrderItems { get; set; } = new List<OrderItemtForm>();
        

    }
}
