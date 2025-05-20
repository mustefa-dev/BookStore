using System.ComponentModel.DataAnnotations;

namespace BookStore.DATA.DTOs
{

    public class DistrictForm 
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid GovernorateId { get; set; }

    }
}
