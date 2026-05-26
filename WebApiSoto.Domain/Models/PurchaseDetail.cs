using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSoto.Domain.Models;

public partial class PurchaseDetail
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    
    public virtual Products? Products { get; set; }

    public int? PurchaseId { get; set; }

    public decimal? PurchasePrice { get; set; }

    public decimal? SalePrice { get; set; }

    public decimal? Total { get; set; }

    public int? Quantity { get; set; }

    public virtual Purchase? Purchase { get; set; }
}
