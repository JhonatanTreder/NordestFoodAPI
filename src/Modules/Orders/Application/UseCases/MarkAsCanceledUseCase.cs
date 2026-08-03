using NordesteFoodAPI.Modules.Orders.Domain.Contracts.Repositories;
using NordesteFoodAPI.Modules.Orders.Domain.DTOs.Order;
using NordesteFoodAPI.Shared.Common.Results;
using NordesteFoodAPI.Shared.Exceptions;

namespace NordesteFoodAPI.Modules.Orders.Application.UseCases
{
    public class MarkAsCanceledUseCase
    {
        private readonly IOrderRepository _orderRepository;

        public MarkAsCanceledUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result> MarkAsCanceledAsync(Guid orderId)
        {
            var order = await _orderRepository.FindByIdAsync(orderId);

            if (order is null)
            {
                return Result.Failure(
                    $"O pedido de Id '{orderId}' não foi encontrado",
                    ErrorType.NotFound
                );
            }

            try
            {
                order.Cancel();
            }
            catch (DomainLayerException ex)
            {
                return Result.Failure(
                    ex.Message,
                    ErrorType.Conflict
                );
            }

            var updateOrderResult = await _orderRepository.UpdateAsync(order);

            if (!updateOrderResult.IsSuccess)
            {
                return Result.Failure(
                    updateOrderResult.ErrorMessage ?? $"Ocorreu um erro inesperado ao tentar cancelar o pedido de Id '{orderId}'",
                    updateOrderResult.ErrorType
                );
            }

            return Result.Success();
        }
    }
}
