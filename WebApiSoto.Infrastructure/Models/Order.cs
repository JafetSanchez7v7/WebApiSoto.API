using System;
using System.Collections.Generic;

namespace WebApiSoto.Infrastructure.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public bool? Delivery { get; set; }

    public bool? IsActive { get; set; }

    public bool? HalfPayment { get; set; }

    public DateTime? TimeDelivery { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<PersonalizedProduct> PersonalizedProducts { get; set; } = new List<PersonalizedProduct>();
}
