using AutoMapper;
using BookStore.DATA.DTOs;
using BookStore.DATA.DTOs.Cart;
using BookStore.Entities;
using BookStore.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services
{
    public interface ICartServices
    {
        Task<(CartDto? cartDto, string? ErrorMessage)> GetMyCart(Guid userId);
        Task<(string? SuccessMessage, string? ErrorMessage)> AddToCart(Guid userId, CartForm cartForm);
        Task<(string? message, string? error)> RemoveFromCart(Guid userId, Guid bookId, int quantity);
        Task<(string? message, string? error)> UpdateCartItem(Guid userId, Guid bookId, int newQuantity);
        Task<(string? message, string? error)> ClearCart(Guid userId);
    }

    public class CartServices : ICartServices
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public CartServices(IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            _mapper = mapper;
            _repository = repositoryWrapper;
        }

        public async Task<(CartDto? cartDto, string? ErrorMessage)> GetMyCart(Guid userId)
        {
            var user = await _repository.User.Get(x => x.Id == userId);
            if (user == null) return (null, "User not found");
            var cart = await _repository.Cart
                .Get(x => x.UserId == userId, include: q => q.Include(c => c.Items).ThenInclude(i => i.Book));
            if (cart == null)
                return (null, "Cart not found");

            cart.TotalAmount = cart.Items.Sum(item => item.PriceAtAddTime * item.Quantity);
            var cartDto = _mapper.Map<CartDto>(cart);
            return (cartDto, null);
        }

        public async Task<(string? SuccessMessage, string? ErrorMessage)> AddToCart(Guid userId, CartForm cartForm)
        {
            var user = await _repository.User.Get(x => x.Id == userId);
            if (user == null) return (null, "User not found");
            if (cartForm.Items == null || cartForm.Items.Count == 0)
                return (null, "Cart must contain at least one item.");

            var bookIds = cartForm.Items.Select(i => i.BookId).ToList();
            var books = await _repository.Book.GetAll(b => bookIds.Contains(b.Id));
            var bookDict = books.data.ToDictionary(b => b.Id);

            foreach (var item in cartForm.Items)
            {
                if (!bookDict.ContainsKey(item.BookId))
                    return (null, $"Book with ID {item.BookId} does not exist.");
                if (bookDict[item.BookId]?.Stock < item.Quantity)
                    return (null, $"Not enough stock for book '{bookDict[item.BookId]?.Name ?? "Unknown"}'.");
            }

            var cart = await _repository.Cart.Get(x => x.UserId == userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    Items = new List<CartProduct>()
                };
                await _repository.Cart.Add(cart);
            }

            foreach (var incomingItem in cartForm.Items)
            {
                var existingItem = await _repository.CartProduct.Get(x =>
                    x.BookId == incomingItem.BookId && x.CartId == cart.Id);
                var book = bookDict[incomingItem.BookId];

                if (existingItem != null)
                {
                    existingItem.Quantity += incomingItem.Quantity;
                    await _repository.CartProduct.Update(existingItem);
                }
                else
                {
                    var newItem = new CartProduct
                    {
                        BookId = incomingItem.BookId,
                        CartId = cart.Id,
                        Quantity = incomingItem.Quantity,
                        PriceAtAddTime = book.Price
                    };
                    await _repository.CartProduct.Add(newItem);
                }
            }

            return ("Books added to cart successfully", null);
        }

        public async Task<(string? message, string? error)> RemoveFromCart(Guid userId, Guid bookId, int quantity)
        {
            var user = await _repository.User.Get(x => x.Id == userId);
            if (user == null) return (null, "User not found");
            var cart = await _repository.Cart.Get(x => x.UserId == userId);
            if (cart == null)
                return (null, "Cart not found");

            var item = await _repository.CartProduct.Get(x => x.BookId == bookId && x.CartId == cart.Id);
            if (item == null)
                return (null, "Book not found in cart");

            if (item.Quantity > quantity)
            {
                item.Quantity -= quantity;
                await _repository.CartProduct.Update(item);
                return ("Book quantity updated successfully", null);
            }
            else if (item.Quantity == quantity)
            {
                await _repository.CartProduct.SoftDelete(item.Id);
                return ("Book removed from cart successfully", null);
            }
            else
            {
                return (null, "Requested quantity exceeds available quantity");
            }
        }

        public async Task<(string? message, string? error)> UpdateCartItem(Guid userId, Guid bookId, int newQuantity)
        {
            var user = await _repository.User.Get(x => x.Id == userId);
            if (user == null) return (null, "User not found");
            if (newQuantity < 0)
                return (null, "Quantity cannot be negative.");

            var cart = await _repository.Cart.Get(x => x.UserId == userId);
            if (cart == null) return (null, "Cart not found.");

            var item = await _repository.CartProduct.Get(x => x.CartId == cart.Id && x.BookId == bookId);
            if (item == null) return (null, "Item not found in cart.");

            if (newQuantity == 0)
                await _repository.CartProduct.SoftDelete(item.Id);
            else
            {
                item.Quantity = newQuantity;
                await _repository.CartProduct.Update(item);
            }

            return ("Quantity updated.", null);
        }

        public async Task<(string? message, string? error)> ClearCart(Guid userId)
        {
            var user = await _repository.User.Get(x => x.Id == userId);
            if (user == null) return (null, "User not found");
            var cart = await _repository.Cart.Get(x => x.UserId == userId, include: q => q.Include(c => c.Items));
            if (cart == null)
                return (null, "Cart not found");

            foreach (var item in cart.Items)
            {
                await _repository.CartProduct.SoftDelete(item.Id);
            }

            return ("Cart cleared successfully.", null);
        }
    }
}