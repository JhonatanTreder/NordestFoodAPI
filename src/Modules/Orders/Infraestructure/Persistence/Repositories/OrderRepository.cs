using Microsoft.EntityFrameworkCore;
using NordesteFoodAPI.Modules.Orders.Domain.Contracts;
using NordesteFoodAPI.Modules.Orders.Domain.Entities;
using NordesteFoodAPI.Shared.Common.Results;
using NordesteFoodAPI.Shared.Infraestructure.Persistence;
using System.Data.Common;

namespace NordesteFoodAPI.Modules.Orders.Infraestructure.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _dbContext;

        public OrderRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<Order>> CreateAsync(Order order)
        {
            try
            {
                await _dbContext.Orders.AddAsync(order);
                await _dbContext.SaveChangesAsync();

                return Result<Order>.Success(order);
            }
            catch (DbUpdateException ex)
            {
                return Result<Order>.Failure(
                    $"Erro ao criar o pedido: {ex.Message}",
                    ErrorType.DatabaseError
                );
            }
        }

        public async Task<Result<Order>> FindByIdAsync(Guid orderId)
        {
            try
            {
                var order = await _dbContext.Orders
                    .Include(o => o.Items)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order is null)
                {
                    return Result<Order>.Failure(
                        $"O pedido com Id '{orderId}' não foi encontrado",
                        ErrorType.NotFound
                    );
                }

                return Result<Order>.Success(order);
            }
            catch (DbUpdateException ex)
            {
                return Result<Order>.Failure(
                    $"Erro ao criar o pedido: {ex.Message}",
                    ErrorType.DatabaseError
                );
            }
        }
    }
}
