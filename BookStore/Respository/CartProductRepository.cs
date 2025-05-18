using AutoMapper;
using BookStore.DATA;
using BookStore.Entities;
using BookStore.Interface;

namespace BookStore.Repository
{

    public class CartProductRepository : GenericRepository<CartProduct , Guid> , ICartProductRepository
    {
        public CartProductRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
