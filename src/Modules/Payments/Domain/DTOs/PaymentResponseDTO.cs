namespace NordesteFoodAPI.Modules.Payments.Domain.DTOs
{
    public record PaymentResponseDTO(
        Guid PaymentId,
        Guid OrderId,
        string PaymentStatus,
        string PaymentMethod, 
        string PaymentProvider,
        string TransactionRef, 
        DateTime RequestedAt, 
        DateTime ProcessedAt
    );
}
