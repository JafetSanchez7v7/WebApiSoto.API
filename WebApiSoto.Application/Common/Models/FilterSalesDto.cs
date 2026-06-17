using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiSoto.Application.Common.Models
{
    public class FilterSalesDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 8;
        public string? CustomerName { get; set; }
        [Range(0, 10000, ErrorMessage = "Por Favor ingrese un numero mayor a 0")]
        public decimal? MaxTotal { get; set; }
        [Range(0, 10000, ErrorMessage = "Por Favor ingrese un numero mayor a 0")]
        public decimal? MinTotal { get; set; }
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
    }
}
