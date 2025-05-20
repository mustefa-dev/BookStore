    using BookStore.DATA.DTOs;

namespace BookStore.DATA.DTOs
{

    public class CityDto : BaseDto<Guid>
    {
        public string? Name { get; set; }
        public string? DistrictName { get; set; }

    }
}
