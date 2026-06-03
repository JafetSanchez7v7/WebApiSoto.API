using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Domain.Models;

namespace WebApiSoto.Application.Common.DTOs.Invoices
{
    public class InvoiceDto
    {
        public int InvoiceId { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? SaleId { get; set; }
        
        

        public bool? IsPrinted { get; set; }

        public DateTime? PrintedDate { get; set; }

        public decimal? TotalAmount { get; set; }

        public virtual ICollection<InvoiceDetailsDto> InvoiceDetails { get; set; } = new List<InvoiceDetailsDto>();
    }
}
