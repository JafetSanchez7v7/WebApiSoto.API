using System;
using System.Collections.Generic;

namespace WebApiSoto.Infrastructure.Models;

public partial class PersonalizedProduct
{
    public int PersonalizedId { get; set; }

    public int? OrderId { get; set; }

    public int? CustomerId { get; set; }

    public string? Description { get; set; }

    public DateTime? CreationDate { get; set; }

    public decimal? SalePrice { get; set; }

    public int? ProductId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual ICollection<Personalization> Personalizations { get; set; } = new List<Personalization>();
}
