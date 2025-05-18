namespace BookStore.Entities
{
    public class Cart : BaseEntity<Guid>
    {
        public Guid? UserId { get; set; }
        public AppUser? User { get; set; }

        public List<CartProduct> Items { get; set; } = new();

        public decimal TotalAmount { get; set; }
    }
}