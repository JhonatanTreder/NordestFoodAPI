namespace NordesteFoodAPI.Modules.Stocks.Domain.DTOs
{
    public record CreateStockRequestDTO(
        Guid RestaurantId,
        Guid ProductId,
        int Quantity
    );
}
