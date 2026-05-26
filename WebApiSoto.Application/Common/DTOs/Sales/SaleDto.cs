using System;
using System.Collections.Generic;

namespace WebApiSoto.Application.Common.DTOs.Sales
{
    public class SaleDto
    {
        public int SaleId { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
       
        public DateTime? SaleDate { get; set; }
        public decimal? SaleTotal { get; set; }
        public ICollection<SaleDetailsDto> SaleDetails { get; set; } = new List<SaleDetailsDto>();
    }
}
