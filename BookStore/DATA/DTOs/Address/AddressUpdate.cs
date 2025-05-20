namespace BookStore.DATA.DTOs
{
    public class AddressUpdate
    {
        public string? Name { get; set; }
        public string? FullAddress { get; set; }
        public double? Latidute { get; set; }
        public double? Longitude { get; set; }
        public bool? IsMain { get; set; }
    }
}