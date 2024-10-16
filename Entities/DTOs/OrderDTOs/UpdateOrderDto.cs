using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.OrderDTOs
{
    public class UpdateOrderDto
    {
        public OrderStatus Status { get; set; }
    }
}
