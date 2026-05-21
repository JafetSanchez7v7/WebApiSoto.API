using System;
using System.Collections.Generic;

namespace WebApiSoto.Domain.Models;

public partial class InvoiceDetail
{
    public int Id { get; set; }

    public int? InvoiceId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? SalePrice { get; set; }

    public decimal? LineTotal { get; set; }

    public virtual Invoice? Invoice { get; set; }
}
