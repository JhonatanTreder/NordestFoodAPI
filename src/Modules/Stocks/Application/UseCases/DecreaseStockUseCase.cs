using NordesteFoodAPI.Modules.Stocks.Domain.Contracts;
using NordesteFoodAPI.Modules.Stocks.Domain.DTOs;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Stocks.Application.UseCases
{
    public class DecreaseStockUseCase
    {
        private readonly IStockRepository _stockRepository;

        public DecreaseStockUseCase(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<Result> DecreaseAsync(Guid stockId, int quantity)
        {
            var stock = await _stockRepository.FindByIdAsync(stockId);

            if (stock is null)
            {
                return Result.Failure(
                    $"O estoque de Id '{stockId}' não foi encontrado",
                    ErrorType.NotFound
                );
            }

            var decreaseStockResult = await _stockRepository.DecreaseStockAsync(stockId, quantity);

            if (!decreaseStockResult.IsSuccess)
            {
                return Result.Failure(
                    decreaseStockResult.ErrorMessage ?? $"Ocorreu um erro inesperado ao tentar decrementar o estoque de Id '{stockId}'",
                    decreaseStockResult.ErrorType
                );
            }

            return Result.Success();
        }
    }
}
