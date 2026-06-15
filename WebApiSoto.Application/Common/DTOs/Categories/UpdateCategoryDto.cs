using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.DTOs.Categories
{
    public class UpdateCategoryDto
    {
        public string CategoryName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public string? Icon { get; set; }
    }
}