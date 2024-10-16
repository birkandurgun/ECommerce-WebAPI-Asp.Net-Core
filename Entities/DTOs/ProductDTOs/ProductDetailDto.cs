using Entities.Concrete;
using Entities.DTOs.ProductReviewDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.ProductDTOs
{
    public class ProductDetailDto
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; } 
        public int StockQuantity { get; set; }
        public decimal? Weight { get; set; } 
        public string Brand { get; set; } 
        public string CategoryName { get; set; }
        public List<string> ProductImageUrls { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; }
    }

}
