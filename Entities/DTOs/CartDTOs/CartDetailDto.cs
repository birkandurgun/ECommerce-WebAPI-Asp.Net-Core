using Entities.DTOs.CartItemDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.CartDTOs
{
    public class CartDetailDto
    {
        public int UserId { get; set; }
        public List<CartItemDetailDto> CartItems { get; set; } = new List<CartItemDetailDto>();
    }
}
