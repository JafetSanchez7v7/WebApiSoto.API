using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSoto.Domain.Models;

public partial class SaleDetail
{
    public int Id { get; set; }

    public int? SaleId { get; set; }

    public int? ProductId { get; set; }

    [ForeignKey("ProductId")]
    public virtual Products Product {get; set;}

    public int? Quantity { get; set; }

    

    public decimal? LineAmount { get; set; }

  

    public virtual Sale? Sale { get; set; }
}
