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

        public void ConfirmPayment()
        {
            OrderStatus = OrderStatus.PagamentoConfirmado;
            UpdatedAt = DateTime.UtcNow;
        }

        public void StartPreparation()
        {
            if (OrderStatus != OrderStatus.PagamentoConfirmado)
            {
                throw new DomainLayerException("Um pedido precisa estar com o pagamento confirmado para iniciar o preparo.");
            }

            OrderStatus = OrderStatus.EmPreparo;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsReady()
        {
            if (OrderStatus != OrderStatus.EmPreparo)
            {
                throw new DomainLayerException("Um pedido precisa estar em preparo para ser marcado como pronto.");
            }

            OrderStatus = OrderStatus.Pronto;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsdelivered()
        {
            if (OrderStatus != OrderStatus.Pronto)
            {
                throw new DomainLayerException("Um pedido precisa estar pronto para ser marcado como entregue.");
            }

            OrderStatus = OrderStatus.Entregue;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (OrderStatus == OrderStatus.Entregue || OrderStatus == OrderStatus.Cancelado)
            {
                throw new DomainLayerException("Um pedido que já foi cancelado ou já foi entregue não pode ser marcado como cancelado");
            }

            OrderStatus = OrderStatus.Cancelado;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
