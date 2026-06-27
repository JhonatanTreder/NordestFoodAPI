using NordesteFoodAPI.Modules.Restaurants.Domain.Contracts;
using NordesteFoodAPI.Modules.Restaurants.Domain.DTOs;
using NordesteFoodAPI.Modules.Restaurants.Domain.Entities;
using NordesteFoodAPI.Modules.Restaurants.Domain.ValueObjects;
using NordesteFoodAPI.Shared.Infraestructure.Results;

namespace NordesteFoodAPI.Modules.Restaurants.Application.UseCases
{
    public class CreateRestaurantUseCase
    {
        private readonly IRestaurantRepository _restaurantRepository;
        public CreateRestaurantUseCase(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        public async Task<Result<CreateRestaurantResponseDTO>> Create(CreateRestaurantRequestDTO createRestaurantDTO)
        {
            if (!TimeOnly.TryParse(createRestaurantDTO.OpeningTime, out var openingTime) ||
                !TimeOnly.TryParse(createRestaurantDTO.ClosingTime, out var closingTime))
            {
                return Result<CreateRestaurantResponseDTO>.Failure(
                    "Horário de abertura ou fechamento inválido.",
                    ErrorType.ValidationError);
            }

            var existingRestaurant = await _restaurantRepository.FindByNameAsync(createRestaurantDTO.ComercialName);
            if (existingRestaurant != null)
            {
                return Result<CreateRestaurantResponseDTO>.Failure(
                    $"O restaurante com o nome {createRestaurantDTO.ComercialName} já existe",
                    ErrorType.CreateConflict);
            }

            var newRestaurant = new Restaurant()
            {
                Id = Guid.NewGuid(),
                ComercialName = ComercialName.Create(createRestaurantDTO.ComercialName),
                Address = Address.Create(createRestaurantDTO.Address),
                OperationTime = OperationTime.Create(openingTime, closingTime),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var createResult = await _restaurantRepository.CreateRestaurantAsync(newRestaurant);
            return createResult;
        }
    }
}
