using System;
using System.Collections.Generic;

namespace WebApiSoto.Domain.Models;

public partial class Option
{
    public int OptionId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Measurement { get; set; }

    public decimal? Price { get; set; }

    
}
