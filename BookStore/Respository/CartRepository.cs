using AutoMapper;
using BookStore.DATA;
using BookStore.Entities;
using BookStore.Interface;

namespace BookStore.Repository
{

    public class CartRepository : GenericRepository<Cart , Guid> , ICartRepository
    {
        public CartRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
