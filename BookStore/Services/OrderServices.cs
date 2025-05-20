using AutoMapper;
using BookStore.DATA.DTOs;
using BookStore.DATA.DTOs.Order;
using BookStore.Entities;
using BookStore.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services
{
    public interface IOrderService
    {
        Task<(List<OrderDto>? orderDtos, int? totalCount, string? error)> GetAll(OrderFilter filters, Guid userId);

        Task<(OrderDto? orderDto, string? error)> Add(OrderForm orderForm, Guid userId);

        Task<(string? successMessage, string? error)> Approve(Guid id, Guid userId);
        Task<(string? successMessage, string? error)> Delivered(Guid id, Guid userId);
        Task<(string? successMessage, string? error)> Cancel(Guid id, Guid userId);
    }

    public class OrderService : IOrderService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public OrderService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<(List<OrderDto>? orderDtos, int? totalCount, string? error)> GetAll(OrderFilter filters,
            Guid userId   )
        {
            var (orders, totalCount) = await _repositoryWrapper.Order.GetAll<OrderDto>();

            var orderDtos = _mapper.Map<List<OrderDto>>(orders);
            return (orderDtos, totalCount, null);
        }

        public async Task<(OrderDto? orderDto, string? error)> Add(OrderForm orderForm, Guid userId)
        {
            var user = await _repositoryWrapper.User.Get(x => x.Id == userId);
            if (user == null) return (null, "User not found");
            var order = _mapper.Map<Order>(orderForm);
            order.UserId = userId;
            order.OrderStatus = OrderStatus.Pending;
            order.AddressId = user.AddressId;
    
            var createdOrder = await _repositoryWrapper.Order.Add(order);

            if (createdOrder == null) return (null, "Unable to create order");
    
            foreach (var item in orderForm.OrderItems)
            {
                var orderItem = _mapper.Map<OrderItem>(item);
                orderItem.OrderId = createdOrder.Id;
        
                var addedItem = await _repositoryWrapper.OrderItem.Add(orderItem);
                if (addedItem == null)
                    return (null, "Failed to add order item");
            }
            
        
            var orderDto = _mapper.Map<OrderDto>(order);

            return (orderDto, null);
        }

        public async Task<(string? successMessage, string? error)> Approve(Guid id, Guid userId)
        {
            var order = await _repositoryWrapper.Order.Get(x => x.Id == id);

            if (order == null) 
                return (null, "Order not found");
                
            if (order.OrderStatus != OrderStatus.Pending) 
                return (null, "Cannot approve this order - it is not in pending status");

            order.OrderStatus = OrderStatus.Accepted;
            order.DateOfAccepted = DateTime.UtcNow;
            var updatedOrder = await _repositoryWrapper.Order.Update(order);

            if (updatedOrder == null) 
                return (null, "Failed to approve the order");

            return ("Order has been approved successfully", null);
        }

        public async Task<(string? successMessage, string? error)> Delivered(Guid id, Guid userId)
        {
            var order = await _repositoryWrapper.Order.Get(x => x.Id == id);
            
            if (order == null) 
                return (null, "Order not found");
                
            order.OrderStatus = OrderStatus.Delivered;
            order.DateOfDelivered = DateTime.UtcNow;
            var updatedOrder = await _repositoryWrapper.Order.Update(order);

            if (updatedOrder == null) 
                return (null, "Failed to mark order as delivered");
                
            return ("Order has been delivered successfully", null);
        }

        public async Task<(string? successMessage, string? error)> Cancel(Guid id, Guid userId)
        {
            var order = await _repositoryWrapper.Order.Get(x => x.Id == id);
            
            if (order == null) 
                return (null, "Order not found");
                
            if (order.UserId != userId) 
                return (null, "Cannot cancel another user's order");
                
            order.OrderStatus = OrderStatus.Canceled;
            order.DateOfCanceled = DateTime.UtcNow;
            var updatedOrder = await _repositoryWrapper.Order.Update(order);

            if (updatedOrder == null) 
                return (null, "Failed to cancel the order");

            return ("Order has been canceled successfully", null);
        }
    }
}