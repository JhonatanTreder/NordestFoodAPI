using NordesteFoodAPI.Modules.Products.Domain.Contracts;
using NordesteFoodAPI.Modules.Restaurants.Domain.Contracts;
using NordesteFoodAPI.Modules.UnitProducts.Domain.Contracts;
using NordesteFoodAPI.Modules.UnitProducts.Domain.DTOs;
using NordesteFoodAPI.Shared.Common.Results;
using System.Linq;
namespace NordesteFoodAPI.Modules.UnitProducts.Application.UseCases
{
    public class GetRestaurantMenuUseCase
    {
        private readonly IUnitProductRepository _unitProductRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IProductsRepository _productRepository;

        public GetRestaurantMenuUseCase(
            IUnitProductRepository unitProductRepository,
            IRestaurantRepository restaurantRepository,
            IProductsRepository productRepository)
        {
            _unitProductRepository = unitProductRepository;
            _restaurantRepository = restaurantRepository;
            _productRepository = productRepository;
        }

        public async Task<Result<IEnumerable<UnitProductResponseDTO>>> GetMenuAsync(Guid restaurantId)
        {
            var restaurant = await _restaurantRepository.FindByIdAsync(restaurantId);
            if (restaurant is null)
            {
                return Result<IEnumerable<UnitProductResponseDTO>>.Failure(
                    $"O restaurante de Id '{restaurantId}' não foi encontrado",
                    ErrorType.NotFound
                );
            }

            var menuResult = await _unitProductRepository.FindByRestaurantIdAsync(restaurantId);
            if (!menuResult.IsSuccess)
            {
                return Result<IEnumerable<UnitProductResponseDTO>>.Failure(
                    menuResult.ErrorMessage!, menuResult.ErrorType);
            }

            var unitProducts = menuResult.Value!.ToList();
            if (unitProducts.Count == 0)
            {
                return Result<IEnumerable<UnitProductResponseDTO>>.Failure(
                    $"Nenhum item do cardápio foi encontrado para o restaurante de Id '{restaurantId}'",
                    ErrorType.NotFound
                );
            }

            var productIds = unitProducts.Select(up => up.ProductId).Distinct();
            var productsResult = await _productRepository.FindByIdsAsync(productIds);

            if (!productsResult.IsSuccess)
            {
                return Result<IEnumerable<UnitProductResponseDTO>>.Failure(
                    productsResult.ErrorMessage!, productsResult.ErrorType);
            }

            var productsById = productsResult.Value!.ToDictionary(p => p.Id);

            var response = unitProducts.Select(up => new UnitProductResponseDTO(
                up.Id,
                up.ProductId,
                up.RestaurantId,
                up.Price.Value,
                productsById[up.ProductId].ProductName.Value,
                up.IsAvailable,
                up.CreatedAt,
                up.UpdatedAt
            )).ToList();

            return Result<IEnumerable<UnitProductResponseDTO>>.Success(response);
        }
    }
}