
using BookStore.DATA.DTOs;
using BookStore.DATA.DTOs.Cart;
using BookStore.DATA.DTOs.Order;
using BookStore.DATA.DTOs.Statistics;
using BookStore.Services;
using BookStore.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        
        [HttpGet]
        public async Task<ActionResult<Respons<OrderDto>>> GetAll([FromQuery]OrderFilter filters) => Ok(await _orderService.GetAll(filters,Id), filters.PageNumber);

        
        [HttpPost]
        public async Task<ActionResult> Add([FromBody]OrderForm orderForm) => Ok(await _orderService.Add(orderForm, Id));
        
        [HttpPut("{id}/Approve")]
        public async Task<ActionResult> Approve(Guid id) => Ok(await _orderService.Approve(id, Id));
        
        [HttpPut("{id}/Delivered")]
        public async Task<ActionResult> Delivered(Guid id) => Ok(await _orderService.Delivered(id, Id));
        
        [HttpPut("{id}/Cancel")]
        public async Task<ActionResult> Cancel(Guid id) => Ok(await _orderService.Cancel(id, Id));
       
        [HttpPost("FromCart")]
        public async Task<ActionResult> CreateOrderFromCart([FromBody] CartToOrderForm form) => 
            Ok(await _orderService.CreateOrderFromCart(Id, form.Note, form.AddressId));     
        
        [HttpGet("statistics")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetStatistics([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
            => Ok(await _orderService.GetOrderStatistics(startDate, endDate));
    }
    
}