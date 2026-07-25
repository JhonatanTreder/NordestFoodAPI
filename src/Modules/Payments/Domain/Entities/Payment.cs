
using NordesteFoodAPI.Modules.Payments.Domain.Enums;

namespace NordesteFoodAPI.Modules.Payments.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public PaymentStatus PaymentStatus { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; }
        public PaymentProvider PaymentProvider { get; private set; }
        public string TransactionRef { get; private set; } = null!;
        public DateTime RequestedAt { get; private set; }
        public DateTime ProcessedAt { get; private set; }

        private Payment() { }

        public static Payment Create(Guid orderId, PaymentMethod paymentMethod, PaymentProvider paymentProvider)
        {
            return new Payment
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                PaymentStatus = PaymentStatus.Pending,
                PaymentMethod = paymentMethod,
                PaymentProvider = paymentProvider,
                TransactionRef = Guid.NewGuid().ToString(),
                RequestedAt = DateTime.UtcNow
            };
        }

        public void MarkAsProcessed()
        {
            PaymentStatus = PaymentStatus.Approved;
            ProcessedAt = DateTime.UtcNow;
        }
    }
}
