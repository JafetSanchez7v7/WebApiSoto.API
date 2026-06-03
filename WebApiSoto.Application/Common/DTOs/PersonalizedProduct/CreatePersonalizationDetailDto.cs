using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.DTOs.PersonalizedProduct
{
    public class CreatePersonalizationDetailDto
    {
        [Required(ErrorMessage = "El ID d la opcion es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID debe ser un número entero positivo.")]
        public int OptionId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser un número entero positivo.")]
        public int Quantity { get; set; }
    }
}
