namespace BookStore.Entities
{
    public class Category : BaseEntity<Guid>
    {
        public string? Name { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}
