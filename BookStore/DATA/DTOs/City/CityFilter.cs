using BookStore.DATA.DTOs;

namespace BookStore.DATA.DTOs
{

    public class CityFilter : BaseFilter 
    {
        public string? Name { get; set; }
        public Guid? DistrictId { get; set; }

    }
}
