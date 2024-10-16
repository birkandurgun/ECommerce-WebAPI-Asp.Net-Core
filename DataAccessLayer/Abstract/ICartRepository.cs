using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserIdAsync(int userId);
        Task<Cart?> RemoveFromCartAsync(int userId, int productId);
        Task<Cart?> UpdateCartItemQuantityAsync(int userId, int productId, bool isIncrement);
        Task<Product?> GetProductByIdAsync(int productId);
        Task SaveChangesAsync();
    }
}
