namespace WebApiSoto.Application.Common.DTOs.Sales
{
    public class SaleDetailsDto
    {
        public int Id { get; set; }
        public int? SaleId { get; set; }
       
        public int ProductId { get; set; }
        public decimal? SalePrice { get; set; }
        public int? Quantity { get; set; }

        public decimal? LineAmount { get; set; }
       
        public string ProductName { get; set; }
    }
}
