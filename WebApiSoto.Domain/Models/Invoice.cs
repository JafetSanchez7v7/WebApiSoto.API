 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSoto.Domain.Models;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public DateTime? CreateDate { get; set; }

    public int? SaleId { get; set; }

    public bool? IsPrinted { get; set; }

    public DateTime? PrintedDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } =  new List<InvoiceDetail>();

    [ForeignKey("SaleId")]
    public  Sale Sale { get; set; }
}
