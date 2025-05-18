using BookStore.Entities;

namespace BookStore.Interface
{
    public interface ICartProductRepository : IGenericRepository<CartProduct , Guid>
    {
         
    }
}
