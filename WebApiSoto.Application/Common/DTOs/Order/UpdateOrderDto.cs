using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.DTOs.Order
{
    public class UpdateOrderDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime TimeDelivery { get; set; }
        public List<UpdateOrderDetailDto> OrderDetails { get; set; } = new();

    }

    public class UpdateOrderDetailDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int Volume { get; set; }
    }
}
