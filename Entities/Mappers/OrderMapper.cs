using Entities.Concrete;
using Entities.DTOs.OrderDTOs;
using Entities.DTOs.OrderItemDTOs;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Mappers
{
    public static class OrderMapper
    {

        public static OrderDetailDto ToOrderDetailDto(this Order order)
        {
            return new OrderDetailDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDetailDto
                {
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };
        }

    }
}
