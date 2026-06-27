using NordesteFoodAPI.Modules.Restaurants.Domain.ValueObjects;

namespace NordesteFoodAPI.Modules.Restaurants.Domain.DTOs
{
    public record CreateRestaurantResponseDTO(
        Guid Id,
        string Address,
        string OpeningTime,
        string ClosingTime,
        bool IsActive,
        DateTime CreatedAt
    );
}
