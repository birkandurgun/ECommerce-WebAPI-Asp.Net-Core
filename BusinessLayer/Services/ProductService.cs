using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using Entities.DTOs.ProductDTOs;
using Entities.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class ProductService:IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<(bool Success, string Message, ProductDetailDto? Product)> CreateProductAsync(CreateProductDto createProductDto)
        {
            if (string.IsNullOrWhiteSpace(createProductDto.Name) ||
                string.IsNullOrWhiteSpace(createProductDto.Description) ||
                createProductDto.Price <= 0 ||
                createProductDto.StockQuantity < 0 ||
                string.IsNullOrWhiteSpace(createProductDto.Brand) ||
                createProductDto.CategoryId <= 0)
            {
                return (false, "Invalid product data. Please ensure all fields are filled out correctly.", null);
            }

            var product = createProductDto.ToProductFromCreate();
            var createdProduct = await _productRepository.CreateAsync(product);

            var productWithDetails = await _productRepository.GetByIdAsync(createdProduct.Id);

            if (productWithDetails == null)
            {
                return (false, "Failed to create product.", null);
            }

            return (true, "Product created successfully.", productWithDetails.ToProductDetailDto());
        }

        public async Task<(bool Success, string Message, ProductDetailDto? Product)> UpdateProductAsync(int id, ProductUpdateDto updateProductDto)
        {
            if (string.IsNullOrWhiteSpace(updateProductDto.Name) ||
                string.IsNullOrWhiteSpace(updateProductDto.Description) ||
                updateProductDto.Price <= 0 ||
                updateProductDto.StockQuantity < 0 ||
                string.IsNullOrWhiteSpace(updateProductDto.Brand) ||
                updateProductDto.CategoryId <= 0)
            {
                return (false, "Invalid product data. Please ensure all fields are filled out correctly.", null);
            }

            var product = updateProductDto.ToProductFromUpdate();
            var updatedProduct = await _productRepository.UpdateAsync(id, product);
            if (updatedProduct == null)
            {
                return (false, "Product not found.", null);
            }

            return (true, "Product updated successfully.", updatedProduct.ToProductDetailDto());
        }

        public async Task<(bool Success, string Message)> DeleteProductAsync(int id)
        {
            var deletedProduct = await _productRepository.DeleteAsync(id);
            if (deletedProduct == null)
            {
                return (false, "Product not found.");
            }

            return (true, "Product deleted successfully.");
        }

        public async Task<(bool Success, string Message, List<ProductDetailDto> Products)> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var productDetailDtos = products.Select(p => p.ToProductDetailDto()).ToList();

            return (true, "Products retrieved successfully.", productDetailDtos);
        }

        public async Task<(bool Success, string Message, ProductDetailDto? Product)> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return (false, "Product not found.", null);

            var productDetailDto = product.ToProductDetailDto();
            return (true, "Product retrieved successfully.", productDetailDto);
        }

    }
}
