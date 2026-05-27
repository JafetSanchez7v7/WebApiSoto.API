using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSoto.Domain.Models;

public partial class Personalization
{
    public int PersonalizationId { get; set; }

    public int? OptionId { get; set; }

    public int? PersonalizedId { get; set; }

    public int? Quantity { get; set; }

    public decimal? SalePrice { get; set; }

    public decimal? SubTotal { get; set; }

    [ForeignKey("OptionId")]
    public virtual Option? Option { get; set; }

    public virtual PersonalizedProduct? Personalized { get; set; }
}
