using Microsoft.EntityFrameworkCore;
using NordesteFoodAPI.Modules.Orders.Domain.Entities;
using NordesteFoodAPI.Modules.Payments.Domain.Contracts.Repositories;
using NordesteFoodAPI.Modules.Payments.Domain.Entities;
using NordesteFoodAPI.Shared.Common.Results;
using NordesteFoodAPI.Shared.Infraestructure.Persistence;

namespace NordesteFoodAPI.Modules.Payments.Infraestructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private AppDbContext _dbContext;

        public PaymentRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<Payment>> CreateAsync(Payment payment)
        {
            try
            {
                await _dbContext.Payments.AddAsync(payment);
                await _dbContext.SaveChangesAsync();

                return Result<Payment>.Success(payment);
            }
            catch (DbUpdateException ex)
            {
                return Result<Payment>.Failure(
                    $"Erro ao criar o pagamento: {ex.Message}",
                    ErrorType.DatabaseError
                );
            }
        }

        public async Task<Result<Payment>> FindByOrderIdAsync(Guid orderId)
        {
            var payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);

            if (payment is null)
            {
                return Result<Payment>.Failure(
                    $"O pagamento não foi encontrado para o pedido de Id '{orderId}'",
                    ErrorType.NotFound
                );
            }

            return Result<Payment>.Success(payment);
        }

        public async Task<Result<Payment>> FindByTransactionRefAsync(string transactionRef)
        {
            var payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.TransactionRef == transactionRef);

            if (payment is null)
            {
                return Result<Payment>.Failure(
                    $"O pagamento não foi encontrado para a transação '{transactionRef}'",
                    ErrorType.NotFound
                );
            }

            return Result<Payment>.Success(payment);
        }
    }
}
