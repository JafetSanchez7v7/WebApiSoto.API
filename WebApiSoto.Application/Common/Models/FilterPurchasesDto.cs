using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.Models
{
    public class FilterPurchasesDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 8; 
        public int? SupplierId { get; set; }
        [Range(0, 10000, ErrorMessage = "Por Favor ingrese un numero mayor a 0")] 
        public decimal? MaxTotal { get; set; }
        [Range(0, 10000, ErrorMessage ="Por Favor ingrese un numero mayor a 0")]
        public decimal? MinTotal { get; set; }
        public DateTime? from { get; set; } 
        public DateTime? to { get; set; }

       

    }
}
