namespace BookStore.DATA.DTOs;

public class GovernorateDto : BaseDto<Guid>
{
    public string? Name { get; set; }
    public decimal? DeliveryPrice { get; set; }

}