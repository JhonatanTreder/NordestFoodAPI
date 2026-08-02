namespace NordesteFoodAPI.Modules.Feedbacks.Domain.DTOs
{
    public record FeedbackResponseDTO(Guid Id, Guid OrderId, string? Comment, string Satisfaction, DateTime CreatedAt);
}
