using NordesteFoodAPI.Modules.Stocks.Domain.Entities;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Stocks.Domain.Contracts
{
    public interface IStockRepository
    {
        Task<Result<Stock>> CreateAsync(Stock stock);
        Task<Result> IncreaseStockAsync(Guid stockId, int quantity);
        Task<Result> DecreaseStockAsync(Guid stockId, int quantity);
        Task<Stock?> FindByIdAsync(Guid stockId);
        Task<Stock?> FindByProductAndRestaurantAsync(Guid productId, Guid restaurantId);
    }
}
