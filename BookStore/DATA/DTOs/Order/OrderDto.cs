using BookStore.DATA.DTOs.User;

namespace BookStore.DATA.DTOs
{

    public class OrderDto : BaseDto<Guid>
    {
        public string? OrderDate { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public string? Note { get; set; }
        
        public List<OrderItemDto>? OrderItemDto { get; set; } 

        public UserDto? Client { get; set; }
        public DateTime? DateOfAccepted { get; set; }
        public DateTime? DateOfCanceled { get; set; }
        public DateTime? DateOfDelivered { get; set; }


    }
}
