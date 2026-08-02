namespace NordesteFoodAPI.Modules.Feedbacks.Domain.DTOs
{
    public record CreateFeedbackRequestDTO(Guid OrderId, string? Comment, string FeedbackStar);
}
