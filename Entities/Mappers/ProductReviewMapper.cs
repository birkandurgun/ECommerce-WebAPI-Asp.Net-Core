using Entities.Concrete;
using Entities.DTOs.ProductReviewDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Mappers
{
    public static class ProductReviewMapper
    {
        public static ProductReview ToReviewFromCreate(this CreateReviewDto createReviewDto)
        {
            return new ProductReview
            {
                ProductId = createReviewDto.ProductId,
                Rating = createReviewDto.Rating,
                Comment = createReviewDto.Comment,
                ReviewDate = createReviewDto.ReviewDate
            };
        }

        public static ProductReviewDetailDto FromReviewToDetailDto(this ProductReview review)
        {
            return new ProductReviewDetailDto
            {
                Id = review.Id,
                ProductName = review.Product.Name,
                Username = review.User.Username,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewDate = review.ReviewDate
            };
        }

        public static ProductReview ToReviewFromUpdate(this UpdateReviewDto updateReviewDto)
        {
            return new ProductReview
            {
                Comment = updateReviewDto.Comment,
                Rating = updateReviewDto.Rating
            };
        }
    }
}
