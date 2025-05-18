using BookStore.Entities;

namespace BookStore.Interface
{
    public interface IOrderItemRepository : IGenericRepository<OrderItem , Guid>
    {
         
    }
}
