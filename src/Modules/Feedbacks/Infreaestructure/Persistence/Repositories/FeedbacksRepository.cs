using Microsoft.EntityFrameworkCore;
using NordesteFoodAPI.Modules.Feedbacks.Domain.Contracts.Repositories;
using NordesteFoodAPI.Modules.Feedbacks.Domain.Entities;
using NordesteFoodAPI.Shared.Common.Results;
using NordesteFoodAPI.Shared.Infraestructure.Persistence;

namespace NordesteFoodAPI.Modules.Feedbacks.Infreaestructure.Persistence.Repositories
{
    public class FeedbacksRepository : IFeedbacksRepository
    {
        private readonly AppDbContext _dbContext;

        public FeedbacksRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<Feedback>> CreateAsync(Feedback feedback)
        {
            try
            {
                await _dbContext.Feedbacks.AddAsync(feedback);
                await _dbContext.SaveChangesAsync();

                return Result<Feedback>.Success(feedback);
            }
            catch (DbUpdateException ex)
            {
                return Result<Feedback>.Failure(
                    $"Ocorreu um erro ao tentar criar o feedback: {ex.Message}",
                    ErrorType.DatabaseError
                );
            }
        }

        public async Task<Feedback?> FindByOrderIdAsync(Guid orderId)
        {
            var feedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(f => f.OrderId == orderId);

            return feedback;
        }
    }
}
