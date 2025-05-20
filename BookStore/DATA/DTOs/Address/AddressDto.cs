using BookStore.DATA.DTOs;

namespace BookStore.DATA.DTOs
{

    public class AddressDto: BaseDto<Guid>
    {
        public string Name { get; set; }
        public string FullAddress { get; set; }
        public double? Latidute { get; set; }
        public double? Longitude { get; set; }
        public bool? IsMain { get; set; }
        public Guid GovernorateId { get; set; }
        public string? GovernorateName { get; set; }
        public Guid DistrictId { get; set; }
        public string DistrictName { get; set; }
        public Guid CityId { get; set; }
        public string? CityName { get; set; }
    }
}
