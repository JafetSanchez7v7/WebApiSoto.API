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
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 8;
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
        public string? CustomerName { get; set; }
        [Range(1, 3, ErrorMessage = "El estado de la compra solicitado no existe")]
        public int? Status { get; set; }

    }
}
