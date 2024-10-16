using BusinessLayer.Abstract;
using BusinessLayer.Services;
using DataAccessLayer.Abstract;
using Entities.DTOs.OrderDTOs;
using Entities.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder()
        {
            var userIdFromToken = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var (success, message, createdOrder) = await _orderService.CreateOrderAsync(userIdFromToken);
            if (!success)
            {
                return BadRequest(message);
            }

            return CreatedAtAction(nameof(GetOrdersByUserId), new { userIdFromToken }, createdOrder);

        }

        [Authorize(Roles = "Customer,Admin")]
        [HttpGet]
        [Route("GetOrdersByUserId")]
        [Route("GetOrdersByUserId/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(int? userId)
        {
            int currentUserId;

            if (User.IsInRole("Customer"))
            {
                currentUserId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            }
            else if (User.IsInRole("Admin") && userId.HasValue)
            {
                currentUserId = userId.Value;
            }
            else
            {
                return Forbid("You do not have permission to access this resource.");
            }

            var (success, message, orders) = await _orderService.GetOrdersByUserIdAsync(currentUserId);

            if (!success)
            {
                return NotFound(message);
            }

            return Ok(orders);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("UpdateOrderStatus/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderDto updateOrderDto)
        {
            if (updateOrderDto == null)
            {
                return BadRequest("Invalid order update data.");
            }

            var (success, message, updatedOrder) = await _orderService.UpdateOrderAsync(orderId, updateOrderDto);
            if (!success)
            {
                return NotFound(message);
            }
            return Ok(updatedOrder);

        }

    }
}
