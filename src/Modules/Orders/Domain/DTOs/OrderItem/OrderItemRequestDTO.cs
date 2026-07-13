namespace NordesteFoodAPI.Modules.Orders.Domain.DTOs.OrderItem
{
    public record OrderItemRequestDTO(
        Guid ProductId,
        int Quantity
    );
}
