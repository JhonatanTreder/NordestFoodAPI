using Microsoft.EntityFrameworkCore;
using NordesteFoodAPI.Modules.Orders.Domain.Contracts.Repositories;
using NordesteFoodAPI.Modules.Orders.Domain.Entities;
using NordesteFoodAPI.Modules.Orders.Domain.Enums;
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

        public async Task<Result<IEnumerable<Order>>> FindByFilterAsync(string? orderChannelFilter, string? orderStatusFilter, int page, int limit)
        {
            IQueryable<Order> query = _dbContext.Orders.Include(o => o.Items);

            if (!string.IsNullOrEmpty(orderChannelFilter))
            {
                if (Enum.TryParse<OrderChannel>(orderChannelFilter, true, out var orderChannel))
                {
                    query = query.Where(o => o.OrderChannel == orderChannel);
                }
            }

            if (!string.IsNullOrEmpty(orderStatusFilter))
            {
                if (Enum.TryParse<OrderStatus>(orderStatusFilter, true, out var orderStatus))
                {
                    query = query.Where(o => o.OrderStatus == orderStatus);
                }
            }

            var orders = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            if (orders.Count == 0)
            {
                return Result<IEnumerable<Order>>.Failure(
                    "Nenhum pedido foi encontrado com os filtros fornecidos.",
                    ErrorType.NotFound
                );
            }

            return Result<IEnumerable<Order>>.Success(orders);
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
