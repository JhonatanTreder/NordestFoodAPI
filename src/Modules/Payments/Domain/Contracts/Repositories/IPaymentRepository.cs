using NordesteFoodAPI.Modules.Payments.Domain.Entities;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Payments.Domain.Contracts.Repositories
{
    public interface IPaymentRepository
    {
        Task<Result<Payment>> CreateAsync(Payment payment);
        Task<Result<Payment>> FindByOrderIdAsync(Guid orderId);
        Task<Result<Payment>> FindByTransactionRefAsync(string transactionRef);
    }
}
