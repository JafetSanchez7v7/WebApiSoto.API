using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.DTOs.PersonalizedProduct
{
    public class PersonalizationDetailDto
    {
        public int PersonalizationId { get; set; }
        public int OptionId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
         public decimal SalePrice { get; set; }
        public decimal SubTotal { get; set; }
    }
}
