using NordesteFoodAPI.Modules.Products.Domain.ValueObjects;

namespace NordesteFoodAPI.Modules.Products.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public ProductName ProductName { get; private set; } = null!;
        public Price ProductPrice { get; private set; } = null!;
        public bool IsFeatured { get; private set; }
        public ProductDescription ProductDescription { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }

        private Product() { }

        public static Product Create(ProductName productName, Price unitPrice, bool isFeatured, ProductDescription productDescription)
        {
            return new Product
            {
                Id = Guid.NewGuid(),
                ProductName = productName,
                ProductPrice = unitPrice,
                IsFeatured = isFeatured,
                CreatedAt = DateTime.UtcNow,
                ProductDescription = productDescription
            };
        }
    }
}
