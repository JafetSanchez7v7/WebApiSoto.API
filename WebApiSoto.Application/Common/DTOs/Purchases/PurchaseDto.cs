using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebApiSoto.Application.Common.DTOs.Purchases
{
    public class PurchaseDto
    {
        public int PurchaseId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set;}
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<PurchaseDetailsDto> PurchaseDetails { get; set; } = new List<PurchaseDetailsDto>();
     }
}
