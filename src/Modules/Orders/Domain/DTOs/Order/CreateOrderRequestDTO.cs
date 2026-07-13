using NordesteFoodAPI.Modules.Orders.Domain.DTOs.OrderItem;

namespace NordesteFoodAPI.Modules.Orders.Domain.DTOs.Order
{
    public record CreateOrderRequestDTO(
        Guid RestaurantId,
        string OrderChannel,
        List<OrderItemRequestDTO> Items
    );
}
