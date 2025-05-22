namespace BookStore.DATA.DTOs
{
    public class OrderDto : BaseDto<Guid>
    {
        public Guid? UserId { get; set; }
        public string? ClientFullName { get; set; }
        public string? ClientEmail { get; set; }
        public string? ClientRole { get; set; }
        public Guid? ClientId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? OrderStatus { get; set; }
        public string? Note { get; set; }
        public DateTime? DateOfAccepted { get; set; }
        public DateTime? DateOfCanceled { get; set; }
        public DateTime? DateOfDelivered { get; set; }
        public decimal TotalPrice { get; set; }
        public string? AddressName { get; set; }
        public string? FullAddress { get; set; }
        public double? Latidute { get; set; }
        public double? Longitude { get; set; }
        public bool? IsMain { get; set; }
        public string? GovernorateName { get; set; }
        public string? DistrictName { get; set; }
        public string? CityName { get; set; }
    
        
        public ICollection<OrderItemDto>? OrderItems { get; set; }
    }
}