using NordesteFoodAPI.Modules.Orders.Domain.ValueObjects;

namespace NordesteFoodAPI.Modules.Orders.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public Quantity Quantity { get; private set; } = null!;
        public decimal UnitPrice { get; private set; }
        public decimal Subtotal { get; private set; }

        private OrderItem() { }

        internal static OrderItem Create(Guid orderId, Guid productId, int quantity, decimal unitPrice)
        {
            return new OrderItem()
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                ProductId = productId,
                Quantity = Quantity.Create(quantity),
                UnitPrice = unitPrice,
                Subtotal = quantity * unitPrice
            };
        }
    }
}
