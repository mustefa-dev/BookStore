namespace BookStore.Entities;

public class Governorate : BaseEntity<Guid>
{
    public string? Name { get; set; }
    public decimal? DeliveryPrice { get; set; }
    
    
}