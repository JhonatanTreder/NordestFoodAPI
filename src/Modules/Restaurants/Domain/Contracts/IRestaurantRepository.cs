using NordesteFoodAPI.Modules.Restaurants.Domain.DTOs;
using NordesteFoodAPI.Modules.Restaurants.Domain.Entities;
using NordesteFoodAPI.Shared.Infraestructure.Results;

namespace NordesteFoodAPI.Modules.Restaurants.Domain.Contracts
{
    public interface IRestaurantRepository
    {
        Task<Result<CreateRestaurantResponseDTO>> CreateRestaurantAsync(Restaurant restaurant);
        Task<Restaurant?> FindByNameAsync(string restaurantName);
    }
}
