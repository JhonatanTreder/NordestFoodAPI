using NordesteFoodAPI.Modules.Products.Domain.Contracts;
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

        public CreateUnitProductUseCase(IUnitProductRepository unitProductRepository, IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
            _unitProductRepository = unitProductRepository;
        }

        public async Task<Result<UnitProductResponseDTO>> CreateAsync(CreateUnitProductRequestDTO unitProductRequestDTO)
        {
            var newUnitProduct = UnitProduct.Create(
                unitProductRequestDTO.RestaurantId,
                unitProductRequestDTO.ProductId,
                UnitPrice.Create(unitProductRequestDTO.Price),
                unitProductRequestDTO.IsAvailable
            );

            var createResult = await _unitProductRepository.CreateAsync(newUnitProduct);

            if (!createResult.IsSuccess)
            {
                return Result<UnitProductResponseDTO>.Failure(createResult.ErrorMessage!, createResult.ErrorType);
            }

            var savedUnitProduct = createResult.Value;

            var productResult = await _productsRepository.FindByIdAsync(savedUnitProduct!.ProductId);

            if (productResult is null) 
            {
                return Result<UnitProductResponseDTO>.Failure(
                    $"O produto de id {savedUnitProduct.ProductId} não foi encontrado",
                    ErrorType.NotFound
                );
            }

            var productName = productResult.ProductName.Value;

            var response = new UnitProductResponseDTO(
                savedUnitProduct!.Id,
                savedUnitProduct.ProductId,
                savedUnitProduct.RestaurantId,
                savedUnitProduct.Price.Value,
                productName,
                savedUnitProduct.IsAvailable,
                savedUnitProduct.CreatedAt,
                savedUnitProduct.UpdatedAt
            );

            return Result<UnitProductResponseDTO>.Success(response);
        }
    }
}
