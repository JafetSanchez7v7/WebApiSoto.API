using System;
using System.Collections.Generic;

namespace WebApiSoto.Infrastructure.Models;

public partial class Inventory
{
    public int InventoryId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public int? ReservedStock { get; set; }

    public decimal? PurchasePrice { get; set; }

    public decimal? SalePrice { get; set; }
}
