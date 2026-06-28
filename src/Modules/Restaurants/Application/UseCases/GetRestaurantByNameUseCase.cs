using NordesteFoodAPI.Modules.Restaurants.Domain.Contracts;
using NordesteFoodAPI.Modules.Restaurants.Domain.DTOs;
using NordesteFoodAPI.Modules.Restaurants.Domain.Entities;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Restaurants.Application.UseCases
{
    public class GetRestaurantByNameUseCase
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public GetRestaurantByNameUseCase(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        public async Task<Result<RestaurantResponseDTO>> Get(string comercialName)
        {
            var restaurant = await _restaurantRepository.FindByNameAsync(comercialName);
            if (restaurant is null)
            {
                return Result<RestaurantResponseDTO>.Failure(
                    $"O restaurante com o nome {comercialName} não foi encontrado",
                    ErrorType.NotFound);
            }

            var response = new RestaurantResponseDTO(
                restaurant.Id,
                restaurant.ComercialName.Value,
                restaurant.Address.Value,
                restaurant.OperationTime.OpeningTime.ToString(),
                restaurant.OperationTime.ClosingTime.ToString(),
                restaurant.IsActive,
                restaurant.CreatedAt
            );

            return Result<RestaurantResponseDTO>.Success(response);
        }
    }
}
