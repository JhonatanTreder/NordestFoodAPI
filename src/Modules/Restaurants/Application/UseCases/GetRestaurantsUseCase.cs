using NordesteFoodAPI.Modules.Restaurants.Domain.Contracts;
using NordesteFoodAPI.Modules.Restaurants.Domain.DTOs;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Restaurants.Application.UseCases
{
    public class GetAllRestaurantsUseCase
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public GetAllRestaurantsUseCase(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        public async Task<Result<IEnumerable<RestaurantResponseDTO>>> GetAll()
        {
            var restaurants = await _restaurantRepository.FindRestaurantsAsync();

            if (restaurants is null)
            {
                return Result<IEnumerable<RestaurantResponseDTO>>.Failure("Nenhum restaurante foi encontrado", ErrorType.NotFound);
            }

            var response = restaurants
                .Where(r => r is not null)
                .Select(r => new RestaurantResponseDTO(
                    r!.Id,
                    r.ComercialName.Value,
                    r.Address.Value,
                    r.OperationTime.OpeningTime.ToString(),
                    r.OperationTime.ClosingTime.ToString(),
                    r.IsActive,
                    r.CreatedAt
                ));

            return Result<IEnumerable<RestaurantResponseDTO>>.Success(response);
        }
    }
}