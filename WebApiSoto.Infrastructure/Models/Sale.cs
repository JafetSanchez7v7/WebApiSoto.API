using System;
using System.Collections.Generic;

namespace WebApiSoto.Infrastructure.Models;

public partial class Sale
{
    public int SaleId { get; set; }

    public int? CustomerId { get; set; }

    public int? OrderId { get; set; }

    public DateTime? SaleDate { get; set; }

    public decimal? SaleTotal { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
}
