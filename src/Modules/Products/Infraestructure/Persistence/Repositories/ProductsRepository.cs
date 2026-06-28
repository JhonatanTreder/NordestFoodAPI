using Microsoft.EntityFrameworkCore;
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
        public async Task<Product?> FindByIdAsync(Guid id)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Result<ProductResponseDTO>> CreateAsync(Product newProduct)
        {
            var existsProductByName = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductName == newProduct.ProductName);

            if (existsProductByName is not null)
            {
                return Result<ProductResponseDTO>.Failure(
                    $"Não foi possível criar um produto: Já existe um produto chamado '{newProduct.ProductName.Value}'",
                    ErrorType.CreateConflict
                );
            }

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

        public async Task<Result<IEnumerable<Product>>> FindByIdsAsync(IEnumerable<Guid> ids)
        {
            var products = await _dbContext.Products
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();

            return Result<IEnumerable<Product>>.Success(products);
        }
    }
}
