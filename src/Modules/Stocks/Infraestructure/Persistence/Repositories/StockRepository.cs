using Microsoft.EntityFrameworkCore;
using NordesteFoodAPI.Modules.Stocks.Domain.Contracts;
using NordesteFoodAPI.Modules.Stocks.Domain.Entities;
using NordesteFoodAPI.Shared.Common.Results;
using NordesteFoodAPI.Shared.Exceptions;
using NordesteFoodAPI.Shared.Infraestructure.Persistence;

namespace NordesteFoodAPI.Modules.Stocks.Infraestructure.Persistence.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly AppDbContext _appDbContext;

        public StockRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Result<Stock>> CreateAsync(Stock stock)
        {
            try
            {
                await _appDbContext.Stocks.AddAsync(stock);
                await _appDbContext.SaveChangesAsync();

                return Result<Stock>.Success(stock);
            }
            catch (DbUpdateException ex)
            {
                return Result<Stock>.Failure(
                    $"Ocorreu um erro ao tentar criar o estoque: {ex.Message}",
                    ErrorType.DatabaseError
                );
            }
        }

        public async Task<Stock?> FindByIdAsync(Guid stockId)
        {
            var stock = await _appDbContext.Stocks.FindAsync(stockId);

            return stock;
        }

        public async Task<Stock?> FindByProductAndRestaurantAsync(Guid productId, Guid restaurantId)
        {
            var stock = await _appDbContext.Stocks.FirstOrDefaultAsync(s => s.RestaurantId == restaurantId && s.ProductId == productId);

            return stock;
        }

        public async Task<Result> IncreaseStockAsync(Guid stockId, int quantity)
        {
            try
            {
                var stock = await _appDbContext.Stocks.FindAsync(stockId);

                if (stock is null)
                {
                    return Result.Failure(
                        $"O estoque de Id '{stockId}' não foi encontrado.",
                        ErrorType.NotFound
                    );
                }

                stock.Increase(quantity);

                await _appDbContext.SaveChangesAsync();

                return Result.Success();
            }
            catch (DomainLayerException ex)
            {
                return Result.Failure(
                   ex.Message,
                   ErrorType.BussinessError
                );
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure(
                   $"Ocorreu um erro ao tentar salvar alterações no banco de dados: {ex.Message}",
                   ErrorType.DatabaseError
                );
            }
            catch (Exception ex)
            {
                return Result.Failure(
                   $"Ocorreu um erro inesperado ao adicionar itens ao estoque: {ex.Message}",
                   ErrorType.UnexpectedFailure
                );
            }
        }

        public async Task<Result> DecreaseStockAsync(Guid stockId, int quantity)
        {
            try
            {
                var stock = await _appDbContext.Stocks.FindAsync(stockId);

                if (stock is null)
                {
                    return Result.Failure($"O estoque de Id '{stockId}' não foi encontrado", ErrorType.NotFound);
                }

                stock.Decrease(quantity);

                await _appDbContext.SaveChangesAsync();

                return Result.Success();
            }
            catch (DomainLayerException ex)
            {
                return Result.Failure(
                    ex.Message,
                    ErrorType.BussinessError
                );
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure(
                    $"Ocorreu um erro ao tentar salvar alterações no banco de dados: {ex.Message}",
                    ErrorType.DatabaseError
                );
            }
            catch (Exception ex)
            {
                return Result.Failure(
                    $"Ocorreu um erro inesperado ao tentar remover itens do estoque: {ex.Message}",
                    ErrorType.UnexpectedFailure
                );
            }
        }
    }
}
