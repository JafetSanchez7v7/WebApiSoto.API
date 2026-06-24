using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSoto.Domain.Models;

public partial class PersonalizedProduct
{
    public int PersonalizedId { get; set; }

    public int? CustomerId { get; set; }
    public string? Description { get; set; }
    public DateTime? CreationDate { get; set; }
    public decimal? SalePrice { get; set; }
    public int? ProductId { get; set; }
    [ForeignKey("ProductId")]
    public virtual Products? Products { get; set; }
    [ForeignKey("CustomerId")]
    public virtual Customers? Customer { get; set; }
    public virtual ICollection<Personalization> Personalizations { get; set; } = new List<Personalization>();
}
