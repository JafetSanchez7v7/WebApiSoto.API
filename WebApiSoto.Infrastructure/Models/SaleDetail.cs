using System;
using System.Collections.Generic;

namespace WebApiSoto.Infrastructure.Models;

public partial class SaleDetail
{
    public int Id { get; set; }

    public int? SaleId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? SalePrice { get; set; }

    public decimal? LineAmount { get; set; }

    public string? Volume { get; set; }

    public virtual Sale? Sale { get; set; }
}
