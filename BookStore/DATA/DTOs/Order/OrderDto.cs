namespace BookStore.DATA.DTOs
{
    public class OrderDto : BaseDto<Guid>
    {
        public string? OrderDate { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public string? Note { get; set; }
        public AddressDto? Address { get; set; }

        public List<OrderItemDto>? OrderItems { get; set; } 

        public string? ClientFullName { get; set; }
        public string? ClientEmail { get; set; }
        public string? ClientRole { get; set; }
        public Guid ClientId { get; set; }
        
        public DateTime? DateOfAccepted { get; set; }
        public DateTime? DateOfCanceled { get; set; }
        public DateTime? DateOfDelivered { get; set; }
    }
}