using NordesteFoodAPI.Modules.Products.Domain.Contracts;
using NordesteFoodAPI.Modules.Products.Domain.DTOs;
using NordesteFoodAPI.Modules.Products.Domain.Entities;
using NordesteFoodAPI.Shared.Common.Results;
using NordesteFoodAPI.Shared.Infraestructure.Persistence;

namespace NordesteFoodAPI.Modules.Products.Infraestructure.Persistence.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<ProductResponseDTO>> CreateAsync(Product newProduct)
        {
            await _dbContext.Products.AddAsync(newProduct);
            await _dbContext.SaveChangesAsync();

            var createResponse = new ProductResponseDTO(
                newProduct.Id,
                newProduct.ProductName.Value,
                newProduct.ProductDescription.Value,
                newProduct.ProductPrice.Value,
                newProduct.IsFeatured
            );

            return Result<ProductResponseDTO>.Success(createResponse);
        }
    }
}
