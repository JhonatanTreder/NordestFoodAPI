using NordesteFoodAPI.Modules.Products.Domain.Contracts;
using NordesteFoodAPI.Modules.Products.Domain.DTOs;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Products.Application.UseCases
{
    public class GetProductByIdUseCase
    {
        private readonly IProductsRepository _productsRepository;

        public GetProductByIdUseCase(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task<Result<ProductResponseDTO>> FindByIdAsync(Guid id)
        {
            var product = await _productsRepository.FindByIdAsync(id);

            if (product is null)
            {
                return Result<ProductResponseDTO>.Failure(
                    $"O produto de Id '{id}' não foi encontrado",
                    ErrorType.NotFound
                );
            }

            var response = new ProductResponseDTO(
                product.Id,
                product.ProductName.Value,
                product.ProductDescription.Value,
                product.ProductPrice.Value,
                product.IsFeatured
            );

            return Result<ProductResponseDTO>.Success(response);
        }
    }
}
