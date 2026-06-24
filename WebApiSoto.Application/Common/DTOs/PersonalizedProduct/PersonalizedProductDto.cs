using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.DTOs.PersonalizedProduct
{
    public class PersonalizedProductDto
    {
        public int PersonalizedId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal SalePrice { get; set; }
        public List<PersonalizationDetailDto> PersonalizationDetails { get; set; } = new();
    }
}
