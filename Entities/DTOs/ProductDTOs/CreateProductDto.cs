using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.ProductDTOs
{
    public class CreateProductDto
    {
        public string Name { get; set; } 
        public string Description { get; set; }
        public decimal Price { get; set; } 
        public int StockQuantity { get; set; }
        public decimal? Weight { get; set; } 
        public string Brand { get; set; } 
        public int CategoryId { get; set; }
        public List<string> ProductImageUrls { get; set; }
    }

}
