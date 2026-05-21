using System;
using System.Collections.Generic;

namespace WebApiSoto.Infrastructure.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public decimal? SalePrice { get; set; }

    public string? Volume { get; set; }

    public int? Quantity { get; set; }

    public decimal? Total { get; set; }

    public virtual Order? Order { get; set; }
}
