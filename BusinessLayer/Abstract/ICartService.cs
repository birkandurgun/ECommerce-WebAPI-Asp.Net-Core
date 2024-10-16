using Entities.DTOs.CartDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ICartService
    {
        Task<(bool Success, string Message, CartDetailDto? Cart)> GetCartByUserIdAsync(int userId);
        Task<(bool Success, string Message, CartDetailDto? Cart)> AddProductToCartAsync(int userId, int productId);
        Task<(bool Success, string Message)> RemoveFromCartAsync(int userId, int productId);
        Task<(bool Success, string Message, CartDetailDto? Cart)> UpdateCartItemQuantityAsync(int userId, int productId, bool isIncrement);
    }
}
