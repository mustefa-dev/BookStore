namespace BookStore.Entities
{
    public class CartProduct : BaseEntity<Guid>
    {
        public Guid CartId { get; set; }
        public Cart? Cart { get; set; }

        public Guid BookId { get; set; }
        public Book? Book { get; set; }

        public int Quantity { get; set; }
        public int PriceAtAddTime { get; set; }
    }

}
