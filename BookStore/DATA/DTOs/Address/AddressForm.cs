namespace BookStore.DATA.DTOs
{

    public class AddressForm 
    {
        public string Name { get; set; }
        public string FullAddress { get; set; }
        public double? Latidute { get; set; }
        public double? Longitude { get; set; }
        public bool? IsMain { get; set; }
        public Guid GovernorateId { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
    }
}
