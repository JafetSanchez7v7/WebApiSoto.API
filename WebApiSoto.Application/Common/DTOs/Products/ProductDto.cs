using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.DTOs.Products
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
       
        public string? Description { get; set; }
        public string? CategoryName { get; set; }
        public string? SupplierName { get; set; }
        public bool IsActive { get; set; }
        
    }
}
