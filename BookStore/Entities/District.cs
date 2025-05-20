namespace BookStore.Entities;

public class District : BaseEntity<Guid>
{
    public string? Name { get; set; }
    public Governorate? Governorate { get; set; }
    public Guid? GovernorateId { get; set; }
    public ICollection<City>? Cities { get; set; }
    public ICollection<Address>? Addresses { get; set; }
}