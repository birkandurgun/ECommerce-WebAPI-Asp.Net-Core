using BusinessLayer.Abstract;
using BusinessLayer.Services;
using DataAccessLayer.Abstract;
using DataAccessLayer.Repositories;
using Entities.DTOs.ProductReviewDTOs;
using Entities.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductReviewController : ControllerBase
    {
        private readonly IProductReviewService _productReviewService;

        public ProductReviewController(IProductReviewService productReviewService)
        {
            _productReviewService = productReviewService;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("GetAllReviews")]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _productReviewService.GetAllReviewsAsync();
            return Ok(reviews);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetReviewById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productReviewService.GetReviewByIdAsync(id);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Review);
        }

        [Authorize]
        [HttpGet]
        [Route("ReviewsFromUser")]
        [Route("ReviewsFromUser/{userId}")]
        public async Task<IActionResult> GetByUserId(int? userId)
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

            var result = await _productReviewService.GetReviewsByUserIdAsync(currentUserId);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Reviews);
        }

        [AllowAnonymous]
        [HttpGet("ReviewsForProduct/{productId}")]
        public async Task<IActionResult> GetByProductId([FromRoute] int productId)
        {
            var result = await _productReviewService.GetReviewsByProductIdAsync(productId);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Reviews);
        }

        [Authorize]
        [HttpPost("CreateReview")]
        public async Task<IActionResult> Create([FromBody] CreateReviewDto createReviewDto)
        {
            int userIdFromToken = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var result = await _productReviewService.CreateReviewAsync(createReviewDto,userIdFromToken);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Review?.Id }, result.Review);
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateReview/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateReviewDto updateReviewDto)
        {
            int currentUserId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            bool isAdmin = User.IsInRole("Admin");

            var result = await _productReviewService.UpdateReviewAsync(id, updateReviewDto, currentUserId, isAdmin);

            if (!result.Success)
            {
                if (result.Message == "You are not authorized to update this review.")
                {
                    return Forbid(result.Message);
                }
                return NotFound(result.Message);
            }

            return Ok(result.Review);
        }


        [Authorize]
        [HttpDelete("DeleteReview/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int currentUserId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            bool isAdmin = User.IsInRole("Admin");

            var result = await _productReviewService.DeleteReviewAsync(id, currentUserId, isAdmin);

            if (!result.Success)
            {
                if (result.Message == "You are not authorized to delete this review.")
                {
                    return Forbid(result.Message);
                }
                return NotFound(result.Message);
            }

            return NoContent();
        }

    }
}
