using AutoMapper;
using BookStore.DATA;
using BookStore.Entities;
using BookStore.Interface;

namespace BookStore.Repository
{

    public class OrderItemRepository : GenericRepository<OrderItem , Guid> , IOrderItemRepository
    {
        public OrderItemRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
