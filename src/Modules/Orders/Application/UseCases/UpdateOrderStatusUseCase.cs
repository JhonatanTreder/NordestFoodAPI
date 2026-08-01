using NordesteFoodAPI.Modules.Orders.Domain.Contracts.Repositories;
using NordesteFoodAPI.Modules.Orders.Domain.DTOs.Order;
using NordesteFoodAPI.Modules.Orders.Domain.Enums;
using NordesteFoodAPI.Shared.Common.Results;
using NordesteFoodAPI.Shared.Exceptions;

namespace NordesteFoodAPI.Modules.Orders.Application.UseCases
{
    public class UpdateOrderStatusUseCase
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderStatusUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result> UpdateStatusAsync(Guid orderId, UpdateOrderStatusRequestDTO updateOrderStatusDTO)
        {
            var repositoryResult = await _orderRepository.FindByIdAsync(orderId);

            if (!repositoryResult.IsSuccess)
            {
                return Result.Failure(
                    repositoryResult.ErrorMessage ?? $"Ocorreu um erro inesperado ao tentar buscar pelo pedido de Id '{orderId}'",
                    repositoryResult.ErrorType
                );
            }

            var order = repositoryResult.Value!;

            var isSuccessOrderStatusConversion = Enum.TryParse<OrderStatus>(updateOrderStatusDTO.OrderStatus, true, out var orderStatus);

            if (!isSuccessOrderStatusConversion)
            {
                return Result.Failure(
                    $"Não foi possível realizar a conversão do status do pedido '{updateOrderStatusDTO.OrderStatus}'.",
                    ErrorType.ValidationError
                );
            }

            try
            {
                switch (orderStatus)
                {
                    case OrderStatus.EmPreparo:
                        order.StartPreparation();
                        break;

                    case OrderStatus.Pronto:
                        order.MarkAsReady();
                        break;

                    case OrderStatus.Entregue:
                        order.MarkAsdelivered();
                        break;

                    case OrderStatus.Cancelado:
                        order.Cancel();
                        break;

                    default:
                        return Result.Failure(
                            $"O status '{orderStatus}' não é uma transição válida.",
                            ErrorType.ValidationError
                        );
                }

                var updateOrderResult = await _orderRepository.UpdateAsync(order);

                if (!updateOrderResult.IsSuccess)
                {
                    return Result.Failure(
                        updateOrderResult.ErrorMessage ?? $"Ocorreu um erro inesperado ao atualizar o pedido de Id '{orderId}'.",
                        updateOrderResult.ErrorType
                    );
                }

                return Result.Success();
            }
            catch (DomainLayerException ex)
            {
                return Result.Failure(ex.Message, ErrorType.UnexpectedFailure);
            }
        }
    }
}
