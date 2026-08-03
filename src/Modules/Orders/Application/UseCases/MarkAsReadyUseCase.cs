using NordesteFoodAPI.Modules.Orders.Domain.Contracts.Repositories;
using NordesteFoodAPI.Shared.Common.Results;
using NordesteFoodAPI.Shared.Exceptions;

namespace NordesteFoodAPI.Modules.Orders.Application.UseCases
{
    public class MarkAsReadyUseCase
    {
        private readonly IOrderRepository _orderRepository;

        public MarkAsReadyUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result> MarkAsReadyAsync(Guid orderId)
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
                order.MarkAsReady();
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
                    updateOrderResult.ErrorMessage ?? $"Ocorreu um erro inesperado ao tentar marcar o pedido de Id '{orderId}' como preparado",
                    updateOrderResult.ErrorType
                );
            }

            return Result.Success();
        }
    }
}
