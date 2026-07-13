namespace NordesteFoodAPI.Modules.UnitProducts.Domain.DTOs
{
    public record CreateUnitProductRequestDTO(
       Guid ProductId,
       Guid RestaurantId,
       decimal Price,
       bool IsAvailable
   );
}
