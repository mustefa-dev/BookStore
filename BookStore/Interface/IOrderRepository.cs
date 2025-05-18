using BookStore.Entities;

namespace BookStore.Interface
{
    public interface IOrderRepository : IGenericRepository<Order , Guid>
    {
         
    }
}
