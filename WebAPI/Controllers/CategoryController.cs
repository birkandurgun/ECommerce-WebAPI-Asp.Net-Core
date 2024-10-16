using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Repositories;
using Entities.Concrete;
using Entities.DTOs.CategoryDTOs;
using Entities.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("GetAllCategories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var (success, message, categories) = await _categoryService.GetAllAsync();
            if (!success)
            {
                return BadRequest(message);
            }
            return Ok(categories);
        }

        [HttpGet("GetById/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var (success, message, category) = await _categoryService.GetByIdAsync(id);
            if (!success)
            {
                return NotFound(message);
            }
            return Ok(category);
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> Add([FromBody] CreateCategoryDto createCategoryDto)
        {
            var (success, message, category) = await _categoryService.CreateAsync(createCategoryDto);
            if (!success)
            {
                return BadRequest(message);
            }
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        [HttpPut("UpdateCategory/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            var (success, message, category) = await _categoryService.UpdateAsync(id, updateCategoryDto);
            if (!success)
            {
                return NotFound(message);
            }
            return Ok(category);
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var (success, message) = await _categoryService.DeleteAsync(id);
            if (!success)
            {
                return NotFound(message);
            }
            return NoContent();
        }
    }
}
