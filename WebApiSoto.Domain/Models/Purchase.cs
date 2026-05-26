using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSoto.Domain.Models;

public partial class Purchase
{
    public int PurchaseId { get; set; }

    public int SupplierId { get; set; }
    [ForeignKey("SupplierId")]
    public virtual Suppliers? Suppliers { get; set; }
    public DateTime? Date { get; set; }

    public decimal? TotalAmount { get; set; }

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();
}
