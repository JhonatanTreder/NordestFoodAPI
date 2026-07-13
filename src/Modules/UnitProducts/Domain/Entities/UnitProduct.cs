using NordesteFoodAPI.Modules.UnitProducts.Domain.ValueObjects;

namespace NordesteFoodAPI.Modules.UnitProducts.Domain.Entities
{
    public class UnitProduct
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid RestaurantId { get; private set; }
        public Guid ProductId { get; private set; }
        public bool IsAvailable { get; private set; }
        public UnitPrice Price { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private UnitProduct() { }

        public static UnitProduct Create(Guid restaurantId, Guid productId, UnitPrice price, bool isAvailable)
        {
            return new UnitProduct
            {
                Id = Guid.NewGuid(),
                RestaurantId = restaurantId,
                ProductId = productId,
                Price = price,
                IsAvailable = isAvailable,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}
