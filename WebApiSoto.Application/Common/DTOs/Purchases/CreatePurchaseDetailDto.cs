using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.DTOs.Purchases
{   public class CreatePurchaseDetailDto
    {
        [Required(ErrorMessage = "El ID del producto es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID debe ser un número entero positivo.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser un número entero positivo.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "El precio de compra es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser un número mayor a cero.")]
        public decimal PurchasePrice { get; set; }

    }

}
