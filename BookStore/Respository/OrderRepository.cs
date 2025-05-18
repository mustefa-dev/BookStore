using AutoMapper;
using BookStore.DATA;
using BookStore.Entities;
using BookStore.Interface;

namespace BookStore.Repository
{

    public class OrderRepository : GenericRepository<Order , Guid> , IOrderRepository
    {
        public OrderRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
