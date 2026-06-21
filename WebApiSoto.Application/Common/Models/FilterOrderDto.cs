using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.Models
{
    public class FilterOrderDto
    {
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
        public string? CustomerName { get; set; }
        public int? Status { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 8;
    }
}
