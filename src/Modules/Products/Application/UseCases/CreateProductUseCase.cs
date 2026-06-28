using NordesteFoodAPI.Modules.Products.Domain.Contracts;
using NordesteFoodAPI.Modules.Products.Domain.DTOs;
using NordesteFoodAPI.Modules.Products.Domain.Entities;
using NordesteFoodAPI.Modules.Products.Domain.ValueObjects;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Products.Application.UseCases
{
    public class CreateProductUseCase
    {
        private readonly IProductsRepository _productRepository;

        public CreateProductUseCase(IProductsRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<ProductResponseDTO>> CreateAsync(CreateProductRequestDTO productRequestDTO)
        {
            var newProduct = Product.Create(
                ProductName.Create(productRequestDTO.ProductName),
                Price.Create(productRequestDTO.UnitPrice),
                false,
                ProductDescription.Create(productRequestDTO.ProductDescription)
            );

            var createResult = await _productRepository.CreateAsync(newProduct);

            return createResult;
        }
    }
}
