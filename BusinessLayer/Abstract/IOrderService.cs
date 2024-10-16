using Entities.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IOrderService
    {
        Task<(bool Success, string Message, OrderDetailDto? Order)> CreateOrderAsync(int userId);
        Task<(bool success, string message, OrderDetailDto? order)> GetOrderByIdAsync(int orderId);
        Task<(bool success, string message, List<OrderDetailDto> orders)> GetOrdersByUserIdAsync(int userId);
        Task<(bool success, string message, OrderDetailDto? order)> UpdateOrderAsync(int orderId, UpdateOrderDto updateOrderDto);
    }
}
