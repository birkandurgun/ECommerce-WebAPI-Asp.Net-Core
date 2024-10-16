using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.ProductReviewDTOs
{
    public class CreateReviewDto
    {
        public int ProductId { get; set; }
        public int Rating { get; set; } 
        public string Comment { get; set; } 
        public DateTime ReviewDate { get; set; } = DateTime.Now; 
    }
}
