using System.ComponentModel.DataAnnotations;

namespace BookStore.DATA.DTOs
{

    public class CityForm 
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public Guid? DistrictId { get; set; }
    }
}
