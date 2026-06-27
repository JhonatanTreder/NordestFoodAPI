namespace NordesteFoodAPI.Modules.Restaurants.Domain.DTOs
{
    public record RestaurantResponseDTO(
        Guid Id,
        string ComercialName,
        string Address,
        string OpeningTime,
        string ClosingTime,
        bool IsActive,
        DateTime CreatedAt
    );
}
