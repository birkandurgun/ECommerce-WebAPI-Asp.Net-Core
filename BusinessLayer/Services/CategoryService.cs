using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using Entities.DTOs.CategoryDTOs;
using Entities.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<(bool Success, string Message, List<CategoryDetailDto> Categories)> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoryDtos = categories.Select(c => c.ToCategoryDetailDto()).ToList();

            return (true, "Categories retrieved successfully.", categoryDtos);
        }

        public async Task<(bool Success, string Message, CategoryDetailDto? Category)> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return (false, "Category not found.", null);
            }

            var categoryDetailDto = category.ToCategoryDetailDto();
            return (true, "Category retrieved successfully.", categoryDetailDto);
        }

        public async Task<(bool Success, string Message, CategoryDetailDto? Category)> CreateAsync(CreateCategoryDto createCategoryDto)
        {
            if (string.IsNullOrWhiteSpace(createCategoryDto.Name))
            {
                return (false, "Category name cannot be empty.", null);
            }

            if (await _categoryRepository.CategoryExistsByNameAsync(createCategoryDto.Name))
            {
                return (false, "Category with the same name already exists.", null);
            }

            var category = createCategoryDto.ToCategoryFromCreateCategoryDto();
            var createdCategory = await _categoryRepository.CreateAsync(category);

            var categoryDetailDto = createdCategory.ToCategoryDetailDto();
            return (true, "Category created successfully.", categoryDetailDto);
        }

        public async Task<(bool Success, string Message, CategoryDetailDto? Category)> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto)
        {

            if (string.IsNullOrWhiteSpace(updateCategoryDto.Name))
            {
                return (false, "Category name cannot be empty.", null);
            }

            if (await _categoryRepository.CategoryExistsByNameAsync(updateCategoryDto.Name))
            {
                var existingCategory = await _categoryRepository.GetByIdAsync(id);
                if (existingCategory?.Name != updateCategoryDto.Name)
                {
                    return (false, "Category with the same name already exists.", null);
                }
            }

            var updatedCategory = await _categoryRepository.UpdateAsync(id, updateCategoryDto);
            if (updatedCategory == null)
            {
                return (false, "Category not found.", null);
            }

            var categoryDetailDto = updatedCategory.ToCategoryDetailDto();
            return (true, "Category updated successfully.", categoryDetailDto);
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var category = await _categoryRepository.DeleteAsync(id);
            if (category == null)
            {
                return (false, "Category not found.");
            }

            return (true, "Category deleted successfully.");
        }

    }
}
