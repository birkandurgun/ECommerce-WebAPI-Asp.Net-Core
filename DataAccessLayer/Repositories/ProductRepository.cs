using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using Entities.Concrete;
using Entities.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ECommerceWithWebAPIDbContext _context;
        public ProductRepository(ECommerceWithWebAPIDbContext context) 
        {
            _context = context;
        }
        public async Task<Product> CreateAsync(Product product)
        {
            if (product.ProductImages != null && product.ProductImages.Any())
            {
                var productImages = product.ProductImages.Select(img => new ProductImage { ImageUrl = img.ImageUrl }).ToList();

                product.ProductImages = productImages;
            }
            else
            {
                product.ProductImages = new List<ProductImage>();
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> DeleteAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if(product == null) { return null; }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .ToListAsync();
        }


        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);
        }


        public async Task<Product?> UpdateAsync(int id, Product product)
        {
            var existingProduct = await _context.Products
                                        .Include(p => p.Category)
                                        .Include(p => p.ProductImages)
                                        .FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct == null) { 
                return null;
            }
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.Weight = product.Weight;
            existingProduct.Brand = product.Brand;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.UpdatedAt = product.UpdatedAt;

            if (product.ProductImages != null && product.ProductImages.Any())
            {
                existingProduct.ProductImages.Clear();

                foreach (var productImage in product.ProductImages)
                {
                    existingProduct.ProductImages.Add(new ProductImage { ImageUrl = productImage.ImageUrl });
                }
            }

            await _context.SaveChangesAsync();
            return existingProduct;
        }
    }
}
