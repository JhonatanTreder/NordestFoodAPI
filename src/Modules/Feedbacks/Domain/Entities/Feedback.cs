using NordesteFoodAPI.Modules.Feedbacks.Domain.Enums;
using NordesteFoodAPI.Modules.Orders.Domain.Entities;

namespace NordesteFoodAPI.Modules.Feedbacks.Domain.Entities
{
    public class Feedback
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public Order Order { get; private set; } = null!;
        public string Comment { get; private set; } = string.Empty;
        public FeedbackStar Satisfaction { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Feedback() { }

        public static Feedback Create(Guid orderId, string? comment, FeedbackStar satisfaction)
        {
            return new Feedback()
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                Comment = comment ?? string.Empty,
                Satisfaction = satisfaction,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
