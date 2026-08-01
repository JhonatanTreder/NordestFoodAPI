using NordesteFoodAPI.Modules.Products.Domain.Contracts;
using NordesteFoodAPI.Modules.Restaurants.Domain.Contracts;
using NordesteFoodAPI.Modules.Stocks.Domain.Contracts;
using NordesteFoodAPI.Modules.Stocks.Domain.DTOs;
using NordesteFoodAPI.Modules.Stocks.Domain.Entities;
using NordesteFoodAPI.Modules.UnitProducts.Domain.Contracts;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Stocks.Application.UseCases
{
    public class CreateStockUseCase
    {
        private readonly IStockRepository _stockRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IUnitProductRepository _unitProductRepository;

        public CreateStockUseCase(
            IStockRepository stockRepository,
            IRestaurantRepository restaurantRepository,
            IProductsRepository productsRepository,
            IUnitProductRepository unitProductRepository
            )
        {
            _stockRepository = stockRepository;
            _restaurantRepository = restaurantRepository;
            _productsRepository = productsRepository;
            _unitProductRepository = unitProductRepository;
        }

        public async Task<Result<StockResponseDTO>> CreateAsync(CreateStockRequestDTO createStockRequestDTO)
        {
            var restaurantId = createStockRequestDTO.RestaurantId;
            var restaurant = await _restaurantRepository.FindByIdAsync(restaurantId);

            if (restaurant is null)
            {
                return Result<StockResponseDTO>.Failure(
                    $"Não foi possível criar o estoque: O restaurante de Id '{restaurantId}' não foi encontrado.",
                    ErrorType.NotFound
                );
            }

            var productId = createStockRequestDTO.ProductId;
            var product = await _productsRepository.FindByIdAsync(productId);

            if (product is null)
            {
                return Result<StockResponseDTO>.Failure(
                  $"Não foi possível criar o estoque: O produto de Id '{productId}' não foi encontrado.",
                  ErrorType.NotFound
                );
            }

            var unitProduct = await _unitProductRepository.FindByProductAndRestaurantAsync(restaurantId, productId);
            var productName = product.ProductName.Value;

            if (unitProduct is null)
            {
                return Result<StockResponseDTO>.Failure(
                    $"Não foi possível criar o estoque: O produto '{productName}' não foi registrado no restaurante de Id '{restaurantId}'.",
                    ErrorType.NotFound
                );
            }

            var stock = Stock.Create(restaurantId, productId, createStockRequestDTO.Quantity);

            var createStockResult = await _stockRepository.CreateAsync(stock);

            if (!createStockResult.IsSuccess)
            {
                return Result<StockResponseDTO>.Failure(
                    createStockResult.ErrorMessage ?? $"Ocorreu um erro inesperado ao tentar criar um estoque para o produto '{productName}' no restaurante de Id '{restaurantId}'.",
                    createStockResult.ErrorType
                );
            }

            var stockData = createStockResult.Value!;

            var stockResponseDTO = new StockResponseDTO(
                stockData.Id,
                stockData.RestaurantId,
                stockData.ProductId,
                stockData.Quantity.Value,
                stockData.UpdatedAt
            );

            return Result<StockResponseDTO>.Success(stockResponseDTO);
        }
    }
}
