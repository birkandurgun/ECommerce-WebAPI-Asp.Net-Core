using BusinessLayer.Abstract;
using BusinessLayer.Services;
using DataAccessLayer.Abstract;
using Entities.DTOs.ProductDTOs;
using Entities.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [AllowAnonymous]
        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<List<ProductDetailDto>>> GetAll()
        {
            var (success, message, products) = await _productService.GetAllProductsAsync();
            if (!success)
                return BadRequest(message);

            return Ok(products);
        }

        [AllowAnonymous]
        [HttpGet("GetProductById/{id}")]
        public async Task<ActionResult<ProductDetailDto>> GetById(int id)
        {
            var (success, message, product) = await _productService.GetProductByIdAsync(id);
            if (!success)
                return NotFound(message);

            return Ok(product);
        }

        [HttpPost("CreateProduct")]
        public async Task<ActionResult<ProductDetailDto>> Create([FromBody] CreateProductDto createProductDto)
        {
            var (success, message, createdProduct) = await _productService.CreateProductAsync(createProductDto);
            if (!success)
                return BadRequest(message);

            return CreatedAtAction(nameof(GetById), new { id = createdProduct?.Id }, createdProduct);
        }

        [HttpPut("UpdateProduct/{id}")]
        public async Task<ActionResult<ProductDetailDto>> Update(int id, [FromBody] ProductUpdateDto updateProductDto)
        {
            var (success, message, updatedProduct) = await _productService.UpdateProductAsync(id, updateProductDto);
            if (!success)
                return BadRequest(message);

            return Ok(updatedProduct);
        }

        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await _productService.DeleteProductAsync(id);
            if (!success)
                return NotFound(message);

            return NoContent();
        }

    }
}
