using Entities.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ICategoryService
    {
        Task<(bool Success, string Message, List<CategoryDetailDto> Categories)> GetAllAsync();
        Task<(bool Success, string Message, CategoryDetailDto? Category)> GetByIdAsync(int id);
        Task<(bool Success, string Message, CategoryDetailDto? Category)> CreateAsync(CreateCategoryDto createCategoryDto);
        Task<(bool Success, string Message, CategoryDetailDto? Category)> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }
}
