using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using Entities.Concrete;
using Entities.DTOs.OrderDTOs;
using Entities.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class OrderService:IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;

        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }

        public async Task<(bool Success, string Message, OrderDetailDto? Order)> CreateOrderAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null || !cart.CartItems.Any())
            {
                return (false, "Cart is empty or does not exist.", null);
            }

            var order = new Order
            {
                UserId = userId,
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Product.Price
                }).ToList()
            };

            order.TotalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);

            var createdOrder = await _orderRepository.CreateOrderAsync(order);

            cart.CartItems.Clear();
            await _cartRepository.SaveChangesAsync();

            return (true, "Order created successfully.", createdOrder.ToOrderDetailDto());
        }


        public async Task<(bool success, string message, OrderDetailDto? order)> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return (false, "Order not found.", null);
            }

            return (true, "Order retrieved successfully.", order.ToOrderDetailDto());
        }

        public async Task<(bool success, string message, List<OrderDetailDto> orders)> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            var orderDtos = orders.Select(o => o.ToOrderDetailDto()).ToList();
            return (true, "Orders retrieved successfully.", orderDtos);
        }

        public async Task<(bool success, string message, OrderDetailDto? order)> UpdateOrderAsync(int orderId, UpdateOrderDto updateOrderDto)
        {
            var updatedOrder = await _orderRepository.UpdateOrderAsync(orderId, updateOrderDto);
            if (updatedOrder == null)
            {
                return (false, "Order not found.", null);
            }

            return (true, "Order updated successfully.", updatedOrder.ToOrderDetailDto());
        }

    }
}
