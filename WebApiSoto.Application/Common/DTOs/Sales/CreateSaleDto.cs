using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApiSoto.Application.Common.DTOs.Sales
{
    public class CreateSaleDto
    {
        [Required(ErrorMessage = "El Id del cliente es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id del cliente debe ser un número positivo")]
        public int CustomerId { get; set; }

        

        [Required(ErrorMessage = "Los detalles de la venta son obligatorios")]
        [MinLength(1, ErrorMessage = "Debe haber al menos un detalle de venta")]
        public List<CreateSaleDetailDto> SaleDetails { get; set; }
    }
}
