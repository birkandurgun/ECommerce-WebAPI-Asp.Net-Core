using Entities.Concrete;
using Entities.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryDetailDto ToCategoryDetailDto(this Category category)
        {
            return new CategoryDetailDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public static Category ToCategoryFromCategoryDto(this CategoryDetailDto categoryDto)
        {
            return new Category
            {
                Id = categoryDto.Id,
                Name = categoryDto.Name
            };
        }

        public static Category ToCategoryFromCreateCategoryDto(this CreateCategoryDto categoryDto) {
            return new Category
            {
                Name = categoryDto.Name,
            };
        }
    }
}
