using NordesteFoodAPI.Modules.Payments.Domain.Enums;

namespace NordesteFoodAPI.Modules.Payments.Domain.DTOs
{
    public record CreatePaymentRequestDTO(Guid OrderId, string PaymentMethod, string PaymentProvider);
}
