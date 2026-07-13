using NordesteFoodAPI.Modules.Orders.Domain.DTOs.OrderItem;

namespace NordesteFoodAPI.Modules.Orders.Domain.DTOs.Order
{
    public record OrderResponseDTO(
        Guid UserId,
        Guid Id,
        Guid RestaurantId,
        string OrderStatus,
        string OrderChannel,
        decimal Total,
        DateTime RequestedAt,
        List<OrderItemResponseDTO> Items
    );
}
