namespace NordesteFoodAPI.Modules.Products.Domain.DTOs
{
    public class CreateProductRequestDTO()
    {
        public string ProductName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public string ProductDescription { get; set; } = null!;
    }
}
