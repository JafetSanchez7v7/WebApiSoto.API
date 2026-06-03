using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.DTOs.Invoices
{
    public class InvoiceDetailsDto
    {
        public int Id { get; set; }

        public int? InvoiceId { get; set; }

        public int? ProductId { get; set; }
        public string ProductName { get; set; }

        public int? Quantity { get; set; }

        
        public decimal? LineTotal { get; set; }
    }
}
