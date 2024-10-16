using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using Entities.Concrete;
using Entities.DTOs.CartDTOs;
using Entities.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class CartService: ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<(bool Success, string Message, CartDetailDto? Cart)> GetCartByUserIdAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                return (false, "Cart not found.", null);
            }

            var cartDetailDto = cart.ToCartDetailDto();
            return (true, "Cart retrieved successfully.", cartDetailDto);
        }

        public async Task<(bool Success, string Message, CartDetailDto? Cart)> AddProductToCartAsync(int userId, int productId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return (false, "Cart not found.", null);
            }

            var product = await _cartRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                return (false, "Product not found.", null);
            }

            var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += 1;
            }
            else
            {
                var cartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = 1,
                    UnitPrice = product.Price
                };
                cart.CartItems.Add(cartItem);
            }

            await _cartRepository.SaveChangesAsync();

            var cartDetailDto = cart.ToCartDetailDto();
            return (true, "Product added to cart successfully.", cartDetailDto);
        }


        public async Task<(bool Success, string Message)> RemoveFromCartAsync(int userId, int productId)
        {
            var cart = await _cartRepository.RemoveFromCartAsync(userId, productId);

            if (cart == null)
            {
                return (false, "Cart or product not found.");
            }

            return (true, "Product removed from cart successfully.");
        }

        public async Task<(bool Success, string Message, CartDetailDto? Cart)> UpdateCartItemQuantityAsync(int userId, int productId, bool isIncrement)
        {
            var cart = await _cartRepository.UpdateCartItemQuantityAsync(userId, productId, isIncrement);

            if (cart == null)
            {
                return (false, "Failed to update cart item quantity.", null);
            }

            var cartDetailDto = cart.ToCartDetailDto();
            return (true, "Cart item quantity updated successfully.", cartDetailDto);
        }

    }
}
