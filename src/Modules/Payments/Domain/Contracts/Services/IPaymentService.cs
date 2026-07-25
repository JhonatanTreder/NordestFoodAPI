using NordesteFoodAPI.Modules.Payments.Domain.DTOs;
using NordesteFoodAPI.Modules.Payments.Domain.Enums;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Payments.Domain.Contracts.Services
{
    public interface IPaymentService
    {
        Task<Result<PaymentStatus>> ProcessAsync(CreatePaymentRequestDTO paymentRequestDTO);
    }
}
