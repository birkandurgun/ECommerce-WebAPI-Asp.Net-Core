using Entities.Concrete;
using Entities.DTOs.CartDTOs;
using Entities.DTOs.CartItemDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Mappers
{
    public static class CartMapper
    {
        public static CartDetailDto ToCartDetailDto(this Cart cart)
        {
            return new CartDetailDto
            {
                UserId = cart.UserId,
                CartItems = cart.CartItems.Select(ci => new CartItemDetailDto
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.Name,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.UnitPrice
                }).ToList()
            };
        }
    }
}
