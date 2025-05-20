using BookStore.DATA.DTOs;

namespace BookStore.DATA.DTOs
{

    public class AddressFilter : BaseFilter 
    {
        public string? Name { get; set; }
        public string? FullAddress { get; set; }
        public Guid? GovernorateId { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? CityId { get; set; }

    }
}
