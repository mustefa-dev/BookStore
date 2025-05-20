namespace BookStore.Entities
{
    public class City : BaseEntity<Guid>
    {
        public string? Name { get; set; }
        public Guid? DistrictId { get; set; }
        public District? District { get; set; }
        public ICollection<Address>? Addresses { get; set; }
    }
}
