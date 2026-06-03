using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.DTOs.Order
{
    public class OrderDetailDto
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }

        public decimal SalePrice { get; set; }
        public int Quantity { get; set; }
        public string? Volume { get; set; }
        public decimal Total { get; set; }
    }
}
