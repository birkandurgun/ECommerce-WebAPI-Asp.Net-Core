using Entities.Concrete;
using Entities.DTOs.ProductDTOs;
using Entities.DTOs.ProductReviewDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Mappers
{
    public static class ProductMapper
    {
        public static ProductDetailDto ToProductDetailDto(this Product product)
        {
            return new ProductDetailDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Weight = product.Weight,
                Brand = product.Brand,
                CategoryName = product.Category.Name, 
                ProductImageUrls = product.ProductImages.Select(pi => pi.ImageUrl).ToList(), 
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }

        public static Product ToProductFromCreate(this CreateProductDto createProductDto)
        {
            return new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                StockQuantity = createProductDto.StockQuantity,
                Weight = createProductDto.Weight,
                Brand = createProductDto.Brand,
                CategoryId = createProductDto.CategoryId,
                CreatedAt = DateTime.UtcNow,
                ProductImages = createProductDto.ProductImageUrls?.Select(url => new ProductImage { ImageUrl = url }).ToList() ?? new List<ProductImage>()
            };
        }

        public static Product ToProductFromUpdate(this ProductUpdateDto updateProductDto)
        {
            return new Product
            {
                Name = updateProductDto.Name,
                Description = updateProductDto.Description,
                Price = updateProductDto.Price,
                StockQuantity = updateProductDto.StockQuantity,
                Weight = updateProductDto.Weight,
                Brand = updateProductDto.Brand,
                CategoryId = updateProductDto.CategoryId,
                ProductImages = updateProductDto.ProductImageUrls != null
            ? updateProductDto.ProductImageUrls.Select(url => new ProductImage { ImageUrl = url }).ToList()
            : new List<ProductImage>(),
                UpdatedAt = updateProductDto.UpdatedAt
            };
        }

    }
}
