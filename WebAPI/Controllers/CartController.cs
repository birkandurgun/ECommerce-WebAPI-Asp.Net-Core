using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using Entities.DTOs.CartDTOs;
using Entities.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("GetCartByUserId")]
        public async Task<IActionResult> GetCartByUserId()
        {
            var userIdFromToken = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _cartService.GetCartByUserIdAsync(userIdFromToken);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Cart);
        }

        [HttpPost("AddProductToCart/{productId}")]
        public async Task<IActionResult> AddProductToCart(int productId)
        {
            var userIdFromToken = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _cartService.AddProductToCartAsync(userIdFromToken, productId);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Cart);
        }

        [HttpDelete("RemoveFromCart/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userIdFromToken = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _cartService.RemoveFromCartAsync(userIdFromToken, productId);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Message);
        }

        [HttpPut("UpdateQuantity/{productId}")]
        public async Task<IActionResult> UpdateCartItemQuantity(int productId, [FromQuery] bool isIncrement)
        {
            var userIdFromToken = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _cartService.UpdateCartItemQuantityAsync(userIdFromToken, productId, isIncrement);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Cart);
        }

    }
}
