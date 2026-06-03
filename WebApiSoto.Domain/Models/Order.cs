using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSoto.Domain.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    [ForeignKey("CustomerId")]

    public virtual Customers Customer { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public int IsActive { get; set; }

    public decimal HalfPayment { get; set; }

    public DateTime TimeDelivery { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

}

