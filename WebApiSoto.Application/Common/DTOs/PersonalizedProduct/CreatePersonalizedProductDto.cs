using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.DTOs.PersonalizedProduct
{
    public class CreatePersonalizedProductDto
    {
        [Required(ErrorMessage = "El Id del cliente es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id del cliente debe ser un número positivo")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "El Id del producto es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id del producto debe ser un número positivo")]
        public int ProductId { get; set; }
        public string? Description { get; set; }
        public List<CreatePersonalizationDetailDto> PersonalizationDetails { get; set; } = new();
    }
}
