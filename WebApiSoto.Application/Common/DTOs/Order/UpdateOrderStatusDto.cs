using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.DTOs.Order
{
    public class UpdateOrderStatusDto
    {
        public int OrderId { get; set; }
        public int IsActive { get; set; }
    }
}
