using NordesteFoodAPI.Modules.Feedbacks.Domain.Entities;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Feedbacks.Domain.Contracts.Repositories
{
    public interface IFeedbacksRepository
    {
        Task<Result<Feedback>> CreateAsync(Feedback feedback);
        Task<Feedback?> FindByOrderIdAsync(Guid orderId);
    }
}
