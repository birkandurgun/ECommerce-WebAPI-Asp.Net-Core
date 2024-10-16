using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IProductReviewRepository
    {
        Task<ProductReview?> GetByIdAsync(int id);
        Task<List<ProductReview>> GetAllAsync();
        Task<List<ProductReview>> GetByProductIdAsync(int productId);
        Task<List<ProductReview>> GetByUserIdAsync(int userId);
        Task<ProductReview> AddAsync(ProductReview productReview);
        Task<ProductReview> UpdateAsync(int id, ProductReview productReview);
        Task<ProductReview?> DeleteAsync(int id);
    }
}
