using NordesteFoodAPI.Modules.Orders.Domain.Enums;
using NordesteFoodAPI.Shared.Exceptions;

namespace NordesteFoodAPI.Modules.Orders.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid RestaurantId { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime RequestedAt { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        public OrderChannel OrderChannel { get; private set; }
        public decimal Total { get; private set; }

        private readonly List<OrderItem> _items = new();
        public IReadOnlyList<OrderItem> Items => _items;

        private Order() { }

        public static Order Create(Guid userId, Guid restaurantId, OrderChannel orderChannel)
        {
            return new Order()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                RestaurantId = restaurantId,
                UpdatedAt = DateTime.UtcNow,
                RequestedAt = DateTime.UtcNow,
                OrderStatus = OrderStatus.AguardandoPagamento,
                OrderChannel = orderChannel,
            };
        }

        public void AddItem(Guid productId, int quantity, decimal unitPrice)
        {
            if (quantity <= 0)
                throw new Exception("A quantidade deve ser maior que zero.");

            var item = OrderItem.Create(Id, productId, quantity, unitPrice);

            _items.Add(item);
            Total += item.Subtotal;

            UpdatedAt = DateTime.UtcNow;
        }
    }
}
