using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using Entities.DTOs.ProductReviewDTOs;
using Entities.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class ProductReviewService:IProductReviewService
    {
        private readonly IProductReviewRepository _productReviewRepository;

        public ProductReviewService(IProductReviewRepository productReviewRepository)
        {
            _productReviewRepository = productReviewRepository;
        }

        public async Task<(bool Success, string Message, ProductReviewDetailDto? Review)> CreateReviewAsync(CreateReviewDto createReviewDto, int userId)
        {
            if (string.IsNullOrWhiteSpace(createReviewDto.Comment) || createReviewDto.Rating < 1 || createReviewDto.Rating > 5)
            {
                return (false, "Invalid review data. Rating must be between 1 and 5, and comment cannot be empty.", null);
            }

            var productReview = createReviewDto.ToReviewFromCreate();
            productReview.UserId = userId;
            var createdReview = await _productReviewRepository.AddAsync(productReview);
            return (true, "Review created successfully.", createdReview.FromReviewToDetailDto());
        }

        public async Task<(bool Success, string Message, ProductReviewDetailDto? Review)> UpdateReviewAsync(int id, UpdateReviewDto updateReviewDto, int currentUserId, bool isAdmin)
        {
            if (string.IsNullOrWhiteSpace(updateReviewDto.Comment) || updateReviewDto.Rating < 1 || updateReviewDto.Rating > 5)
            {
                return (false, "Invalid review data. Rating must be between 1 and 5, and comment cannot be empty.", null);
            }

            var existingReview = await _productReviewRepository.GetByIdAsync(id);

            if (existingReview == null)
            {
                return (false, "Review not found.", null);
            }

            if (!isAdmin && existingReview.UserId != currentUserId)
            {
                return (false, "You are not authorized to update this review.", null);
            }

            var productReview = updateReviewDto.ToReviewFromUpdate();
            var updatedReview = await _productReviewRepository.UpdateAsync(id, productReview);

            if (updatedReview == null)
            {
                return (false, "Review update failed.", null);
            }

            return (true, "Review updated successfully.", updatedReview.FromReviewToDetailDto());
        }

        public async Task<(bool Success, string Message, List<ProductReviewDetailDto> Reviews)> GetAllReviewsAsync()
        {
            var reviews = await _productReviewRepository.GetAllAsync();
            var reviewDtos = reviews.Select(r => r.FromReviewToDetailDto()).ToList();

            return (true, "Reviews retrieved successfully.", reviewDtos);
        }

        public async Task<(bool Success, string Message, ProductReviewDetailDto? Review)> GetReviewByIdAsync(int id)
        {
            var review = await _productReviewRepository.GetByIdAsync(id);
            if (review == null)
            {
                return (false, "Review not found.", null);
            }

            return (true, "Review retrieved successfully.", review.FromReviewToDetailDto());
        }

        public async Task<(bool Success, string Message)> DeleteReviewAsync(int id, int currentUserId, bool isAdmin)
        {
            var existingReview = await _productReviewRepository.GetByIdAsync(id);

            if (existingReview == null)
            {
                return (false, "Review not found.");
            }

            if (!isAdmin && existingReview.UserId != currentUserId)
            {
                return (false, "You are not authorized to delete this review.");
            }

            var deletedReview = await _productReviewRepository.DeleteAsync(id);
            if (deletedReview == null)
            {
                return (false, "Review deletion failed.");
            }

            return (true, "Review deleted successfully.");
        }


        public async Task<(bool Success, string Message, List<ProductReviewDetailDto> Reviews)> GetReviewsByUserIdAsync(int userId)
        {
            var reviews = await _productReviewRepository.GetByUserIdAsync(userId);
            if (reviews == null || !reviews.Any())
            {
                return (false, "No reviews found for this user.", new List<ProductReviewDetailDto>());
            }

            return (true, "Reviews retrieved successfully.", reviews.Select(r => r.FromReviewToDetailDto()).ToList());
        }

        public async Task<(bool Success, string Message, List<ProductReviewDetailDto> Reviews)> GetReviewsByProductIdAsync(int productId)
        {
            var reviews = await _productReviewRepository.GetByProductIdAsync(productId);
            if (reviews == null || !reviews.Any())
            {
                return (false, "No reviews found for this product.", new List<ProductReviewDetailDto>());
            }

            return (true, "Reviews retrieved successfully.", reviews.Select(r => r.FromReviewToDetailDto()).ToList());
        }

    }
}
