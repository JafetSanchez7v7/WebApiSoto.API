using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.DTOs.Order
{
    public class CreateOrderDto
    {
        [Required(ErrorMessage = "El Id del cliente es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id del cliente debe ser un número positivo")]
        public int CustomerId { get; set; }
        public DateTime TimeDelivery { get; set; }
        public List<CreateOrderDetailDto> OrderDetails { get; set; } = new();
    }
}
