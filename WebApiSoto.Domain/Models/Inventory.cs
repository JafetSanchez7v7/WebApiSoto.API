using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSoto.Domain.Models;

public partial class Inventory
{
    public int InventoryId { get; set; }

    public int ProductId { get; set; }

    public int? Quantity { get; set; }

    public int? ReservedStock { get; set; }

    public decimal? PurchasePrice { get; set; }

    public decimal? SalePrice { get; set; }
    [ForeignKey("ProductId")]

    public Products? Product { get; set; }
}
