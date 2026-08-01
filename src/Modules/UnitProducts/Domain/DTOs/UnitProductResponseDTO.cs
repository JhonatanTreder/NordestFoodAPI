namespace NordesteFoodAPI.Modules.UnitProducts.Domain.DTOs
{
    public record UnitProductResponseDTO(
         Guid Id,
         Guid ProductId,
         Guid RestaurantId,
         decimal Price,
         int? StockQuantity,
         string ProductName,
         bool IsAvailable,
         DateTime CreatedAt,
         DateTime UpdatedAt
     );
}
