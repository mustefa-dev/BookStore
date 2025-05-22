using AutoMapper;
using BookStore.DATA.DTOs;
using BookStore.DATA.DTOs.Order;
using BookStore.DATA.DTOs.Statistics;
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

        Task<(OrderStatisticsDto? statistics, string? error)> GetOrderStatistics(DateTime? startDate, DateTime? endDate);
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
            var user = await _repositoryWrapper.User.Get(x => x.Id == userId);
            if (user == null) return (null, null, "User not found");

            var (orders, totalCount) = user.Role == UserRole.Admin 
                ? await _repositoryWrapper.Order.GetAll<OrderDto>() 
                : await _repositoryWrapper.Order.GetAll<OrderDto>(x => x.UserId == userId);
    
            var orderDtos = _mapper.Map<List<OrderDto>>(orders);
            return (orderDtos, totalCount, null);
        }


        public async Task<(OrderDto? orderDto, string? error)> Add(OrderForm orderForm, Guid userId)
        {
            var user = await _repositoryWrapper.User.Get(x => x.Id == userId);
            if (user == null) return (null, "User not found");

            var address = await _repositoryWrapper.Address.Get(a => a.Id == orderForm.AddressId);
            if (address == null) return (null, "Address not found");

            var governorate = await _repositoryWrapper.Governorate.Get(g => g.Id == address.GovernorateId);
            if (governorate == null) return (null, "Governorate not found");

            decimal deliveryPrice = 0;
            if (governorate.DeliveryPrice.HasValue) deliveryPrice = governorate.DeliveryPrice.Value;

            decimal totalPrice = 0;
            foreach (var item in orderForm.OrderItems)
            {
                var book = await _repositoryWrapper.Book.Get(b => b.Id == item.BookId);
                if (book == null) return (null, $"Book with ID {item.BookId} does not exist");

                if (book.Stock < item.Quantity)
                    return (null,
                        $"Insufficient quantity for book. Available: {book.Stock}, Requested: {item.Quantity}");

                totalPrice += book.Price * item.Quantity;
            }

            totalPrice += deliveryPrice;

            var order = _mapper.Map<Order>(orderForm);
            order.UserId = userId;
            order.OrderStatus = OrderStatus.Pending;
            order.AddressId = orderForm.AddressId;
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
            var user = await _repositoryWrapper.User.Get(x => x.Id == userId);
            if (user == null) return (null, "User not found");

            if (user.Role != UserRole.Admin) return (null, "Only administrators can approve orders");

            var order = await _repositoryWrapper.Order.Get(x => x.Id == id);
            if (order == null) return (null, "Order not found");

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
            var user = await _repositoryWrapper.User.Get(x => x.Id == userId);
            if (user == null) return (null, "User not found");
            if (user.Role != UserRole.Admin) return (null, "Only administrators can deliver orders");
            var order = await _repositoryWrapper.Order.Get(x => x.Id == id);
            if (order == null) return (null, "Order not found");

            if (order.OrderStatus != OrderStatus.Accepted)
                return (null, "Cannot deliver this order - it is not in pending status");
            order.OrderStatus = OrderStatus.Delivered;
            order.DateOfDelivered = DateTime.UtcNow;
            var updatedOrder = await _repositoryWrapper.Order.Update(order);

            if (updatedOrder == null) return (null, "Failed to mark order as delivered");
            return ("Order has been delivered successfully", null);
        }

        public async Task<(string? successMessage, string? error)> Cancel(Guid id, Guid userId)
        {
            var user = await _repositoryWrapper.User.Get(x => x.Id == userId);
            if (user == null) return (null, "User not found");

            if (user.Id != userId) return (null, "Only the user who created the order can cancel it");
            var order = await _repositoryWrapper.Order.Get(x => x.Id == id);

            if (order == null) return (null, "Order not found");

            if (order.OrderStatus == OrderStatus.Delivered)
                return (null, "Cannot cancel this order - it is already deliverer");
            if (order.UserId != userId) return (null, "Cannot cancel another user's order");

            order.OrderStatus = OrderStatus.Canceled;
            order.DateOfCanceled = DateTime.UtcNow;
            var updatedOrder = await _repositoryWrapper.Order.Update(order);

            if (updatedOrder == null) return (null, "Failed to cancel the order");

            return ("Order has been canceled successfully", null);
        }

        public async Task<(OrderDto? orderDto, string? error)> CreateOrderFromCart(Guid userId, string? note)
        {
            var user = await _repositoryWrapper.User.Get(x => x.Id == userId);
            if (user == null) return (null, "User not found");

            if (user.Id != userId) return (null, "Only the user who created the order can create");

            var cart = await _repositoryWrapper.Cart.Get(x => x.UserId == userId);
            if (cart == null) return (null, "Cart not found");

            var address = await _repositoryWrapper.Address.Get(a => a.Id == user.AddressId);
            if (address == null) return (null, "User address not found");

            var governorate = await _repositoryWrapper.Governorate.Get(g => g.Id == address.GovernorateId);
            if (governorate == null) return (null, "Governorate not found");

            decimal deliveryPrice = 0;
            if (governorate.DeliveryPrice.HasValue) deliveryPrice = governorate.DeliveryPrice.Value;

            var (orderData, totalCount) = await _repositoryWrapper.CartProduct.GetAll(x => x.CartId == cart.Id);
            if (orderData == null || orderData.Count == 0) return (null, "Cart is empty");

            decimal totalPrice = 0;

            foreach (var item in orderData)
            {
                var book = await _repositoryWrapper.Book.Get(b => b.Id == item.BookId);
                if (book == null) return (null, $"Book with ID {item.BookId} does not exist");
                if (book.Stock < item.Quantity)
                    return (null,
                        $"Insufficient quantity for book. Available: {book.Stock}, Requested: {item.Quantity}");
                totalPrice += book.Price * item.Quantity;
            }

            totalPrice += deliveryPrice;

            var order = _mapper.Map<Order>(orderData);

            var createdOrder = await _repositoryWrapper.Order.Add(order);
            if (createdOrder == null) return (null, "Unable to create order");

            foreach (var item in orderData)
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

            foreach (var cartProduct in orderData)
            {
                await _repositoryWrapper.CartProduct.Delete(cartProduct.Id);
            }

            await _repositoryWrapper.Cart.Delete(cart.Id);

            return (orderDto, null);
        }

        public async Task<(OrderStatisticsDto? statistics, string? error)> GetOrderStatistics(DateTime? startDate,
            DateTime? endDate)
        {
            startDate ??= DateTime.UtcNow.AddDays(-30);
            endDate ??= DateTime.UtcNow;

            var (orders, _) = await _repositoryWrapper.Order.GetAll(
                o => o.OrderDate >= startDate && o.OrderDate <= endDate);

            if (orders == null || orders.Count == 0)
                return (new OrderStatisticsDto(), null);

            var statistics = new OrderStatisticsDto
            {
                TotalOrders = orders.Count,
                TotalRevenue = orders.Where(o => o.OrderStatus != OrderStatus.Canceled)
                    .Sum(o => o.TotalPrice),
            };

            statistics.AverageOrderValue = statistics.TotalOrders > 0
                ? statistics.TotalRevenue / statistics.TotalOrders
                : 0;

            var ordersByStatus = orders.GroupBy(o => o.OrderStatus)
                .ToDictionary(g => g.Key.ToString(), g => g.Count());
            statistics.OrdersByStatus = ordersByStatus;

            var ordersByGovernorate = new Dictionary<string, decimal>();
            foreach (var order in orders.Where(o => o.OrderStatus != OrderStatus.Canceled))
            {
                var address = await _repositoryWrapper.Address.Get<AddressDto>(a => a.Id == order.AddressId);
                if (address != null && address.GovernorateName != null)
                {
                    var govName = address.GovernorateName?? "Unknown";
                    if (ordersByGovernorate.ContainsKey(govName))
                        ordersByGovernorate[govName] += order.TotalPrice;
                    else
                        ordersByGovernorate[govName] = order.TotalPrice;
                }
            }

            statistics.RevenueByGovernorate = ordersByGovernorate;

            var topCustomers = orders.GroupBy(o => o.UserId ?? Guid.Empty)
                .Select(g => new TopCustomerDto
                {
                    UserId = g.Key,
                    OrderCount = g.Count(),
                    TotalSpent = g.Where(o => o.OrderStatus != OrderStatus.Canceled)
                        .Sum(o => o.TotalPrice)
                })
                .OrderByDescending(c => c.TotalSpent)
                .Take(5)
                .ToList();

            foreach (var customer in topCustomers)
            {
                var user = await _repositoryWrapper.User.Get(u => u.Id == customer.UserId);
                customer.Name = user?.FullName ?? "Unknown";
            }

            statistics.TopCustomers = topCustomers;

            var dailyRevenue = orders.Where(o => o.OrderStatus != OrderStatus.Canceled)
                .GroupBy<Order, DateTime>(o => o.OrderDate?.Date ?? DateTime.MinValue)
                .ToDictionary(
                    g => g.Key.ToString("yyyy-MM-dd"),
                    g => g.Sum(o => o.TotalPrice)
                );
            statistics.DailyRevenue = dailyRevenue;

            return (statistics, null);
        }
    }
}