using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSoto.Domain.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    public virtual Products Product { get; set; }

    public decimal SalePrice { get; set; }

    public decimal Volume { get; set; }

    public int Quantity { get; set; }

    public decimal Total { get; set; }

    public virtual Order? Order { get; set; }
}
