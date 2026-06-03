using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.DTOs.Order
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal HalfPayment { get; set; }
        public DateTime TimeDelivery { get; set; }
        public int IsActive { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = new();
    }
}
