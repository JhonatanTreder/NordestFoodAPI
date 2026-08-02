using Microsoft.EntityFrameworkCore;
using NordesteFoodAPI.Modules.UnitProducts.Domain.Contracts;
using NordesteFoodAPI.Modules.UnitProducts.Domain.Entities;
using NordesteFoodAPI.Modules.UnitProducts.Domain.ValueObjects;
using NordesteFoodAPI.Shared.Common.Results;
using NordesteFoodAPI.Shared.Infraestructure.Persistence;

namespace NordesteFoodAPI.Modules.UnitProducts.Infraestructure.Persistence.Repositories
{
    public class UnitProductRepository : IUnitProductRepository
    {
        private readonly AppDbContext _dbContext;
        public UnitProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<UnitProduct>> CreateAsync(UnitProduct unitProduct)
        {
            await _dbContext.AddAsync(unitProduct);
            await _dbContext.SaveChangesAsync();

            return Result<UnitProduct>.Success(unitProduct);
        }

        public async Task<UnitProduct?> FindByProductAndRestaurantAsync(Guid productId, Guid restaurantId)
        {
            var unitProduct = await _dbContext.UnitProducts
                .FirstOrDefaultAsync(u =>
                    u.ProductId == productId &&
                    u.RestaurantId == restaurantId
                );

            return unitProduct;
        }

        public async Task<Result<IEnumerable<UnitProduct>>> FindByRestaurantIdAsync(Guid restaurantId)
        {
            var products = await _dbContext.UnitProducts
                .Where(u => u.RestaurantId == restaurantId)
                .ToListAsync();

            return Result<IEnumerable<UnitProduct>>.Success(products);
        }

        public async Task<Result> ActiveUnitProductByIdAsync(Guid unitProductId)
        {
            var unitProduct = await _dbContext.UnitProducts.FirstOrDefaultAsync( u => u.Id == unitProductId);

            if (unitProduct is null)
            {
                return Result.Failure(
                    $"O produto de Id '{unitProductId}' de um restaurante não foi encontrado",
                    ErrorType.NotFound
                );
            }

            unitProduct.Active();

            try
            {
                await _dbContext.SaveChangesAsync();

                return Result.Success();
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure(
                    $"Ocorreu um erro ao tentar ativar o produto de Id '{unitProductId}' de um restaurante: {ex.Message}",
                    ErrorType.DatabaseError
                );
            }
        }
    }
}
