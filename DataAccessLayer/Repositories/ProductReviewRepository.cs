using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ProductReviewRepository : IProductReviewRepository
    {
        private readonly ECommerceWithWebAPIDbContext _context;
        public ProductReviewRepository(ECommerceWithWebAPIDbContext context)
        {
            _context = context;
        }
        public async Task<ProductReview> AddAsync(ProductReview productReview)
        {
            await _context.ProductReviews.AddAsync(productReview);
            await _context.SaveChangesAsync();
            return await _context.ProductReviews
                    .Include(r => r.Product)
                    .Include(r => r.User)
                    .FirstOrDefaultAsync(r => r.Id == productReview.Id);
        }

        public async Task<ProductReview?> DeleteAsync(int id)
        {
            var review = await _context.ProductReviews.FirstOrDefaultAsync(r => r.Id == id);
            if (review == null) { return null; }
            _context.ProductReviews.Remove(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<List<ProductReview>> GetAllAsync()
        {
            return await _context.ProductReviews
                .Include(r => r.User)
                .Include(r => r.Product)
                .ToListAsync();
        }

        public async Task<ProductReview?> GetByIdAsync(int id)
        {
            return await _context.ProductReviews
                .Include(r => r.User)
                .Include(r => r.Product)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<ProductReview>> GetByProductIdAsync(int productId)
        {
            return await _context.ProductReviews
                                 .Where(r => r.ProductId == productId)
                                 .Include(r => r.User)
                                 .Include(r => r.Product)
                                 .ToListAsync();
        }

        public async Task<List<ProductReview>> GetByUserIdAsync(int userId)
        {
            return await _context.ProductReviews
                                 .Where(r => r.UserId == userId)
                                 .Include(r => r.User)
                                 .Include(r => r.Product)
                                 .ToListAsync();
        }

        public async Task<ProductReview> UpdateAsync(int id, ProductReview productReview)
        {
            var existingReview = await _context.ProductReviews.FirstOrDefaultAsync(r => r.Id == id);


            if (existingReview != null)
            {
                existingReview.Rating = productReview.Rating;
                existingReview.Comment = productReview.Comment;

                await _context.SaveChangesAsync();
            }

            return existingReview;
        }
    }
}
