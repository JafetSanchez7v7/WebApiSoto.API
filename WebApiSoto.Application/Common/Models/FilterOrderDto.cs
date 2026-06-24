using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 8;

        [Range(1, 3, ErrorMessage = "El estado de la compra solicitado no existe")]
        public int? Status { get; set; }
    }
}