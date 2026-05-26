using System.ComponentModel.DataAnnotations;

namespace WebApiSoto.Application.Common.DTOs.Sales
{
    public class CreateSaleDetailDto
    {
        [Required(ErrorMessage = "El ID del producto es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID debe ser un número entero positivo.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser un número entero positivo.")]
        public int Quantity { get; set; }

        
    }
}
