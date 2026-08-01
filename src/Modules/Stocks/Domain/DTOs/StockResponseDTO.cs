namespace NordesteFoodAPI.Modules.Stocks.Domain.DTOs
{
    public record StockResponseDTO(
        Guid Id,
        Guid RestaurantId,
        Guid ProductId,
        int Quantity,
        DateTime UpdatedAt
    );
}
