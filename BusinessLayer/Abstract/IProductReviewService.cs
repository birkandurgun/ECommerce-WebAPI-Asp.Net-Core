using Entities.DTOs.ProductReviewDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IProductReviewService
    {
        Task<(bool Success, string Message)> DeleteReviewAsync(int id, int currentUserId, bool isAdmin);
        Task<(bool Success, string Message, ProductReviewDetailDto? Review)> GetReviewByIdAsync(int id);
        Task<(bool Success, string Message, List<ProductReviewDetailDto> Reviews)> GetAllReviewsAsync();
        Task<(bool Success, string Message, ProductReviewDetailDto? Review)> UpdateReviewAsync(int id, UpdateReviewDto updateReviewDto, int currentUserId, bool isAdmin);
        Task<(bool Success, string Message, ProductReviewDetailDto? Review)> CreateReviewAsync(CreateReviewDto createReviewDto, int userId);
        Task<(bool Success, string Message, List<ProductReviewDetailDto> Reviews)> GetReviewsByUserIdAsync(int userId);
        Task<(bool Success, string Message, List<ProductReviewDetailDto> Reviews)> GetReviewsByProductIdAsync(int productId);
    }
}
