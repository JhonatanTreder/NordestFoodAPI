using NordesteFoodAPI.Modules.Stocks.Domain.ValueObjects;
using NordesteFoodAPI.Shared.Exceptions;

namespace NordesteFoodAPI.Modules.Stocks.Domain.Entities
{
    public class Stock
    {
        public Guid Id { get; private set; }
        public Guid RestaurantId { get; private set; }
        public Guid ProductId { get; private set; }
        public Quantity Quantity { get; private set; } = null!;
        public DateTime UpdatedAt { get; private set; }

        private Stock() { }

        public static Stock Create(Guid restaurantId, Guid productId, int quantity)
        {
            return new Stock()
            {
                Id = Guid.NewGuid(),
                RestaurantId = restaurantId,
                ProductId = productId,
                Quantity = Quantity.Create(quantity),
                UpdatedAt = DateTime.UtcNow
            };
        }

        public void Decrease(int quantity)
        {
            if (Quantity.Value < quantity)
            {
                throw new DomainLayerException("Estoque insuficiente para ser decrementado.");
            }

            Quantity = Quantity.Create(Quantity.Value - quantity);
            UpdatedAt = DateTime.UtcNow;
        }

        public void Increase(int quantity)
        {
            if (quantity <= 0)
            {
                throw new DomainLayerException("O valor adicionado ao estoque deve ser maior que 0.");
            }

            Quantity = Quantity.Create(Quantity.Value + quantity);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
