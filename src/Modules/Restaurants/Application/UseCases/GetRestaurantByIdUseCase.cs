using NordesteFoodAPI.Modules.Restaurants.Domain.Contracts;
using NordesteFoodAPI.Modules.Restaurants.Domain.DTOs;
using NordesteFoodAPI.Modules.Restaurants.Domain.ValueObjects;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Restaurants.Application.UseCases
{
    public class GetRestaurantByIdUseCase
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public GetRestaurantByIdUseCase(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        public async Task<Result<RestaurantResponseDTO>> FindByIdAsync(Guid id)
        {
            var restaurant = await _restaurantRepository.FindByIdAsync(id);

            if (restaurant is null)
            {
                return Result<RestaurantResponseDTO>.Failure(
                    $"O restaurante com o ID {id} não foi encontrado",
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
