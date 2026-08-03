using NordesteFoodAPI.Modules.Orders.Domain.Contracts.Repositories;
using NordesteFoodAPI.Modules.Orders.Domain.DTOs.Order;
using NordesteFoodAPI.Modules.Orders.Domain.DTOs.OrderItem;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Orders.Application.UseCases
{
    public class GetOrderByIdUseCase
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderByIdUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result<OrderResponseDTO>> GetByIdAsync(Guid orderId, Guid userId)
        {
            var order = await _orderRepository.FindByIdAsync(orderId);

            if (order is null)
            {
                return Result<OrderResponseDTO>.Failure(
                    $"O pedido de Id '{orderId}' não foi encontrado",
                    ErrorType.NotFound
                );
            }

            //Eu estou entregando o mesmo tipo de mensagem para não vazar informações sobre a existência do pedido para o usuário que não é dono desse pedido em si.
            if (order.UserId != userId)
            {
                return Result<OrderResponseDTO>.Failure(
                   $"O pedido de Id '{orderId}' não foi encontrado",
                   ErrorType.NotFound
                );
            }

            var orderResponseDTO = new OrderResponseDTO(
                UserId: order.UserId,
                Id: order.Id,
                RestaurantId: order.RestaurantId,
                OrderStatus: order.OrderStatus.ToString(),
                OrderChannel: order.OrderChannel.ToString(),
                Total: order.Total,
                RequestedAt: order.RequestedAt,
                Items: order.Items.Select(item => new OrderItemResponseDTO(
                    ProductId: item.ProductId,
                    Quantity: item.Quantity.Value,
                    UnitPrice: item.UnitPrice,
                    Subtotal: item.Subtotal
                )).ToList()
            );

            return Result<OrderResponseDTO>.Success(orderResponseDTO);
        }
    }
}
