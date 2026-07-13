using NordesteFoodAPI.Modules.Orders.Domain.Contracts;
using NordesteFoodAPI.Modules.Orders.Domain.DTOs.Order;
using NordesteFoodAPI.Modules.Orders.Domain.DTOs.OrderItem;
using NordesteFoodAPI.Modules.Orders.Domain.ValueObjects;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Orders.Application.UseCases
{
    public class GetOrderByIdUseCase
    {
        public readonly IOrderRepository _orderRepository;

        public GetOrderByIdUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result<OrderResponseDTO>> GetAsync(Guid orderId)
        {
            var repositoryResponse = await _orderRepository.FindByIdAsync(orderId);

            if (!repositoryResponse.IsSuccess)
            {
                return Result<OrderResponseDTO>.Failure(
                    repositoryResponse.ErrorMessage ??
                    $"Ocorreu um erro inespperado ao tentar buscar pelo pedido de Id '{orderId}'",
                    repositoryResponse.ErrorType
                );
            }

            var order = repositoryResponse.Value!;

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
