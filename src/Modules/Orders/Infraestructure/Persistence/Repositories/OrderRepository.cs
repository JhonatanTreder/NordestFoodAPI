using Microsoft.EntityFrameworkCore;
using NordesteFoodAPI.Modules.Orders.Domain.Contracts.Repositories;
using NordesteFoodAPI.Modules.Orders.Domain.Entities;
using NordesteFoodAPI.Shared.Common.Results;
using NordesteFoodAPI.Shared.Infraestructure.Persistence;

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
                    $"Ocorreu um erro inesperado ao tentar criar o pedido: {ex.Message}",
                    ErrorType.DatabaseError
                );
            }
        }

        public async Task<Order?> FindByIdAsync(Guid orderId)
        {
            var order = await _dbContext.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            return order;
        }

        public async Task<Result<Order>> UpdateAsync(Order order)
        {
            try
            {
                _dbContext.Orders.Update(order);
                await _dbContext.SaveChangesAsync();

                return Result<Order>.Success(order);
            }
            catch (DbUpdateException ex)
            {
                return Result<Order>.Failure(
                    $"Ocorreu um erro ao tentar atualizar o pedido: {ex.Message}",
                    ErrorType.DatabaseError
                );
            }
        }
    }
}
