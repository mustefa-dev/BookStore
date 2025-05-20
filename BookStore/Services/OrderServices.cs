using AutoMapper;
using BookStore.DATA.DTOs;
using BookStore.DATA.DTOs.Order;
using BookStore.Entities;
using BookStore.Interface;

namespace BookStore.Services
{
    public interface IOrderService
    {
        Task<(List<OrderDto>? orderDtos, int? totalCount, string? error)> GetAll(OrderFilter filters, Guid userId);

        Task<(OrderDto? orderDto, string? error)> Add(OrderForm orderForm, Guid userId);

        Task<(string? successMessage, string? error)> Approve(Guid id, Guid userId);
        Task<(string? successMessage, string? error)> Delivered(Guid id, Guid userId);
        Task<(string? successMessage, string? error)> Cancel(Guid id, Guid userId);
        Task<(OrderDto? orderDto, string? error)> CreateOrderFromCart(Guid userId, string? note);
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
            Guid userId)
        {
            var (orders, totalCount) = await _repositoryWrapper.Order.GetAll<OrderDto>();

            var orderDtos = _mapper.Map<List<OrderDto>>(orders);
            return (orderDtos, totalCount, null);
        }

        public async Task<(OrderDto? orderDto, string? error)> Add(OrderForm orderForm, Guid userId)
        {
            var user = await _repositoryWrapper.User.Get(x => x.Id == userId);
            if (user == null) return (null, "User not found");
            decimal totalPrice = 0;

            foreach (var item in orderForm.OrderItems)
            {
                var book = await _repositoryWrapper.Book.Get(b => b.Id == item.BookId);
                if (book == null)
                    return (null, $"Book with ID {item.BookId} does not exist");

                if (book.Stock < item.Quantity)
                    return (null,
                        $"Insufficient quantity for book. Available: {book.Stock}, Requested: {item.Quantity}");

                totalPrice += book.Price * item.Quantity;
            }

            var order = _mapper.Map<Order>(orderForm);
            order.UserId = userId;
            order.OrderStatus = OrderStatus.Pending;
            order.AddressId = user.AddressId;
            order.TotalPrice = totalPrice;
            var createdOrder = await _repositoryWrapper.Order.Add(order);

            if (createdOrder == null) return (null, "Unable to create order");

            foreach (var item in orderForm.OrderItems)
            {
                var book = await _repositoryWrapper.Book.Get(b => b.Id == item.BookId);

                if (book.Stock < item.Quantity)
                    return (null,
                        $"Insufficient quantity for book. Available: {book.Stock}, Requested: {item.Quantity}");

                var orderItem = new OrderItem
                {
                    OrderId = createdOrder.Id,
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                };

                var addedItem = await _repositoryWrapper.OrderItem.Add(orderItem);
                if (addedItem == null)
                    return (null, "Unable to create order item");

                book.Stock -= item.Quantity;
                await _repositoryWrapper.Book.Update(book);
            }

            var orderDto = _mapper.Map<OrderDto>(createdOrder);
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

            if (order.OrderStatus != OrderStatus.Accepted)
                return (null, "Cannot deliver this order - it is not in pending status");
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

            if (order.OrderStatus == OrderStatus.Delivered)
                return (null, "Cannot cancel this order - it is already deliverer");
            if (order.UserId != userId)
                return (null, "Cannot cancel another user's order");

            order.OrderStatus = OrderStatus.Canceled;
            order.DateOfCanceled = DateTime.UtcNow;
            var updatedOrder = await _repositoryWrapper.Order.Update(order);

            if (updatedOrder == null)
                return (null, "Failed to cancel the order");

            return ("Order has been canceled successfully", null);
        }

        public async Task<(OrderDto? orderDto, string? error)> CreateOrderFromCart(Guid userId, string? note)
        {
            var cart = await _repositoryWrapper.Cart.Get(x => x.UserId == userId);
            if (cart == null) return (null, "Cart not found");

            // Get the user separately instead of using cart.User
            var user = await _repositoryWrapper.User.Get(x => x.Id == userId);
            if (user == null) return (null, "User not found");

            var (data, totalCount) = await _repositoryWrapper.CartProduct.GetAll(x => x.CartId == cart.Id);
            if (data == null || data.Count == 0) return (null, "Cart is empty");

            decimal totalPrice = 0;

            foreach (var item in data)
            {
                var book = await _repositoryWrapper.Book.Get(b => b.Id == item.BookId);
                if (book == null) return (null, $"Book with ID {item.BookId} does not exist");
                if (book.Stock < item.Quantity)
                    return (null,
                        $"Insufficient quantity for book. Available: {book.Stock}, Requested: {item.Quantity}");
                totalPrice += book.Price * item.Quantity;
            }

            var order = new Order
            {
                UserId = userId,
                OrderStatus = OrderStatus.Pending,
                Note = note,
                TotalPrice = totalPrice,
                OrderDate = DateTime.UtcNow,
                AddressId = user.AddressId // Use user.AddressId instead of cart.User.AddressId
            };

            var createdOrder = await _repositoryWrapper.Order.Add(order);
            if (createdOrder == null) return (null, "Unable to create order");

            foreach (var item in data)
            {
                var book = await _repositoryWrapper.Book.Get(b => b.Id == item.BookId);
                if (book.Stock < item.Quantity)
                    return (null,
                        $"Insufficient quantity for book. Available: {book.Stock}, Requested: {item.Quantity}");

                var orderItem = new OrderItem
                {
                    OrderId = createdOrder.Id,
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                };

                var addedItem = await _repositoryWrapper.OrderItem.Add(orderItem);
                if (addedItem == null) return (null, "Unable to create order item");

                book.Stock -= item.Quantity;
                await _repositoryWrapper.Book.Update(book);
            }

            var orderDto = _mapper.Map<OrderDto>(createdOrder);

            foreach (var cartProduct in data)
            {
                await _repositoryWrapper.CartProduct.Delete(cartProduct.Id);
            }

            await _repositoryWrapper.Cart.Delete(cart.Id);

            return (orderDto, null);
        }
    }
}