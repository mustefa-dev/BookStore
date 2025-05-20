namespace BookStore.Entities;

public class Governorate : BaseEntity<Guid>
{
    public string? Name { get; set; }
    public ICollection<District>? Districts { get; set; }
    public ICollection<Address>? Addresses { get; set; }
    
    
}