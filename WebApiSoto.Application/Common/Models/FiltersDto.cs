using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.Models
{
    public class FiltersDto
    {
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        [Required(ErrorMessage = "Page Number Is Required")]

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 8;
    }
}
