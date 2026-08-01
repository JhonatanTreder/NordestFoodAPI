using NordesteFoodAPI.Modules.Products.Domain.Contracts;
using NordesteFoodAPI.Modules.Stocks.Domain.Contracts;
using NordesteFoodAPI.Modules.Stocks.Domain.Entities;
using NordesteFoodAPI.Modules.Stocks.Domain.ValueObjects;
using NordesteFoodAPI.Modules.UnitProducts.Domain.Contracts;
using NordesteFoodAPI.Modules.UnitProducts.Domain.DTOs;
using NordesteFoodAPI.Modules.UnitProducts.Domain.Entities;
using NordesteFoodAPI.Modules.UnitProducts.Domain.ValueObjects;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.UnitProducts.Application.UseCases
{
    public class CreateUnitProductUseCase
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IUnitProductRepository _unitProductRepository;
        private readonly IStockRepository _stockRepository;

        public CreateUnitProductUseCase(IUnitProductRepository unitProductRepository, IProductsRepository productsRepository, IStockRepository stockRepository)
        {
            _productsRepository = productsRepository;
            _unitProductRepository = unitProductRepository;
            _stockRepository = stockRepository;
        }

        public async Task<Result<UnitProductResponseDTO>> CreateAsync(CreateUnitProductRequestDTO unitProductRequestDTO)
        {
            var existing = await _unitProductRepository.FindByProductAndRestaurantAsync(
                unitProductRequestDTO.ProductId,
                unitProductRequestDTO.RestaurantId
            );

            if (existing is not null)
            {
                return Result<UnitProductResponseDTO>.Failure(
                    $"Já existe um vínculo do produto de Id '{unitProductRequestDTO.ProductId}' com o restaurante de Id '{unitProductRequestDTO.RestaurantId}'.",
                    ErrorType.Conflict
                );
            }

            var newUnitProduct = UnitProduct.Create(
                unitProductRequestDTO.RestaurantId,
                unitProductRequestDTO.ProductId,
                UnitPrice.Create(unitProductRequestDTO.Price)
            );

            var createResult = await _unitProductRepository.CreateAsync(newUnitProduct);

            if (!createResult.IsSuccess)
            {
                return Result<UnitProductResponseDTO>.Failure(createResult.ErrorMessage!, createResult.ErrorType);
            }

            var savedUnitProduct = createResult.Value!;

            var stock = Stock.Create(
                savedUnitProduct.RestaurantId,
                savedUnitProduct.ProductId,
                unitProductRequestDTO.InitalQuantity
            );

            var createStockResult = await _stockRepository.CreateAsync(stock);

            if (!createStockResult.IsSuccess)
            {
                return Result<UnitProductResponseDTO>.Failure(
                    createStockResult.ErrorMessage ?? $"O produto do restaurante foi criado mas não foi possível criar o estoque",
                    createStockResult.ErrorType
                );
            }

            var productResult = await _productsRepository.FindByIdAsync(savedUnitProduct!.ProductId);

            if (productResult is null)
            {
                return Result<UnitProductResponseDTO>.Failure(
                    $"O produto de id {savedUnitProduct.ProductId} não foi encontrado",
                    ErrorType.NotFound
                );
            }

            var response = new UnitProductResponseDTO(
                savedUnitProduct!.Id,
                savedUnitProduct.ProductId,
                savedUnitProduct.RestaurantId,
                savedUnitProduct.Price.Value,
                stock.Quantity.Value,
                productResult.ProductName.Value,
                savedUnitProduct.IsAvailable,
                savedUnitProduct.CreatedAt,
                savedUnitProduct.UpdatedAt
            );

            return Result<UnitProductResponseDTO>.Success(response);
        }
    }
}
