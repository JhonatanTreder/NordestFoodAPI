namespace NordesteFoodAPI.Modules.Orders.Domain.DTOs.Order
{
    public record OrderFilterRequestDTO(string? OrderChannel, string? OrderStatus, int Page = 1, int Limit = 10);
}
