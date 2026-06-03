using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.DTOs.Order
{
    public class CreateOrderDetailDto
    {
        [Required(ErrorMessage = "El Id del producto es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id del producto debe ser un número positivo")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser un número entero positivo.")]
        public int Quantity { get; set; }
      
        public decimal Volume { get; set; }
    }
}
