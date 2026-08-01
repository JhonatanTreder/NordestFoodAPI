using NordesteFoodAPI.Modules.Stocks.Domain.Contracts;
using NordesteFoodAPI.Modules.Stocks.Domain.DTOs;
using NordesteFoodAPI.Shared.Common.Results;
using NordesteFoodAPI.Shared.Exceptions;

namespace NordesteFoodAPI.Modules.Stocks.Application.UseCases
{
    public class IncreaseStockUseCase
    {
        private readonly IStockRepository _stockRepository;

        public IncreaseStockUseCase(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<Result> IncreaseAsync(Guid stockId, int quantity)
        {
            var stock = await _stockRepository.FindByIdAsync(stockId);

            if (stock is null)
            {
                return Result.Failure(
                    $"O estoque de Id '{stockId}' não foi encontrado.",
                    ErrorType.NotFound
                );
            }

            var increaseStockResult = await _stockRepository.IncreaseStockAsync(stockId, quantity);

            if (!increaseStockResult.IsSuccess)
            {
                return Result.Failure(
                    increaseStockResult.ErrorMessage ?? $"Ocorreu um erro ao tentar incrementar a quantidade do estoque de Id '{stockId}'.",
                    increaseStockResult.ErrorType
                );
            }

            return Result.Success();
        }
    }
}
