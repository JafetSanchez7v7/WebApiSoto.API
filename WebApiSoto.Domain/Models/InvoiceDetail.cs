using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSoto.Domain.Models;

public partial class InvoiceDetail
{
    public int Id { get; set; }

    public int? InvoiceId { get; set; }

    public int? ProductId { get; set; }
    [ForeignKey("ProductId")]
    public virtual Products Product { get; set; }

    public int? Quantity { get; set; }

    

    public decimal? LineTotal { get; set; }
    [ForeignKey("InvoiceId")]
    public virtual Invoice? Invoice { get; set; }
}
