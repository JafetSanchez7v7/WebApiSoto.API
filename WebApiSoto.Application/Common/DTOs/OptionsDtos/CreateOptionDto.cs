using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.DTOs.OptionsDtos
{
    public class CreateOptionDto
    {

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Measurement { get; set; }

        public decimal? Price { get; set; }

    }
}
