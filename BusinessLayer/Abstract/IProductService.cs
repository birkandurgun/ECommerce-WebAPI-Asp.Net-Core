using Entities.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IProductService
    {
        Task<(bool Success, string Message, ProductDetailDto? Product)> CreateProductAsync(CreateProductDto createProductDto);
        Task<(bool Success, string Message, ProductDetailDto? Product)> UpdateProductAsync(int id, ProductUpdateDto updateProductDto);
        Task<(bool Success, string Message)> DeleteProductAsync(int id);
        Task<(bool Success, string Message, List<ProductDetailDto> Products)> GetAllProductsAsync();
        Task<(bool Success, string Message, ProductDetailDto? Product)> GetProductByIdAsync(int id);
    }
}
