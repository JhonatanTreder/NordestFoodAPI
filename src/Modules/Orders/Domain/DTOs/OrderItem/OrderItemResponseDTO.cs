namespace NordesteFoodAPI.Modules.Orders.Domain.DTOs.OrderItem
{
    public record OrderItemResponseDTO(
        Guid ProductId,
        int Quantity,
        decimal UnitPrice,
        decimal Subtotal
    );
}
