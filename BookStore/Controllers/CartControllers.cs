using BookStore.Helpers;
using BookStore.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BookStore.DATA.DTOs;
using BookStore.DATA.DTOs.Cart;
using BookStore.Entities;
using BookStore.Services;

namespace BookStore.Controllers
{
    public class CartsController : BaseController
    {
        private readonly ICartServices _cartServices;

        public CartsController(ICartServices cartServices)
        {
            _cartServices = cartServices;
        }

        [HttpGet("MyCart")]
        public async Task<ActionResult<CartDto>> Get() => Ok(await _cartServices.GetMyCart(Id));
        
        [HttpPost]
        public async Task<ActionResult> AddToCart(CartForm cartForm) => Ok(await _cartServices.AddToCart(Id, cartForm));
        
        [HttpDelete]
        public async Task<ActionResult> DeleteFromCart([FromQuery] Guid ProductId, int Quantity) => Ok(await _cartServices.RemoveFromCart(Id, ProductId, Quantity));
        
        [HttpPut]
        public async Task<ActionResult> UpdateCartItem([FromQuery] Guid ProductId, int Quantity) => Ok(await _cartServices.UpdateCartItem(Id, ProductId, Quantity));
        
        [HttpDelete("Clear")]
        public async Task<ActionResult> ClearCart() => Ok(await _cartServices.ClearCart(Id));
    }
}
