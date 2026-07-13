using NordesteFoodAPI.Modules.Orders.Domain.Entities;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Orders.Domain.Contracts
{
    public interface IOrderRepository
    {
        Task<Result<Order>> CreateAsync(Order order);
        Task<Result<Order>> FindByIdAsync(Guid orderId);
    }
}
