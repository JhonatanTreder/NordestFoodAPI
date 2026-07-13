using NordesteFoodAPI.Modules.UnitProducts.Domain.Entities;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.UnitProducts.Domain.Contracts
{
    public interface IUnitProductRepository
    {
        Task<Result<UnitProduct>> CreateAsync(UnitProduct unitProduct);
        Task<Result<UnitProduct?>> FindByProductAndRestaurantAsync(Guid productId, Guid restaurantId);
        Task<Result<IEnumerable<UnitProduct>>> FindByRestaurantIdAsync(Guid restaurantId);
    }
}
