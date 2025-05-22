namespace BookStore.Entities;

public class Address : BaseEntity<Guid>
{
    public string? Name { get; set; }
    public string? FullAddress { get; set; }
    public double? Latidute { get; set; }
    public double? Longitude { get; set; }
    public bool? IsMain { get; set; }
    public Guid? GovernorateId { get; set; }
    public Governorate? Governorate { get; set; }
    public string? City { get; set; }
    public string? District { get; set; }
    public Guid? AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

}