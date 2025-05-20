using BookStore.DATA.DTOs;

namespace BookStore.DATA.DTOs
{

    public class DistrictFilter : BaseFilter 
    {
        public string? Name { get; set; }
        public Guid? GovernorateId { get; set; }
    }
}
