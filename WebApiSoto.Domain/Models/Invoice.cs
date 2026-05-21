using System;
using System.Collections.Generic;

namespace WebApiSoto.Domain.Models;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public DateTime? CreateDate { get; set; }

    public int? SaleId { get; set; }

    public bool? IsPrinted { get; set; }

    public DateTime? PrintedDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

    public virtual Sale? Sale { get; set; }
}
