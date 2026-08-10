using NordesteFoodAPI.Modules.Orders.Domain.Contracts.Repositories;
using NordesteFoodAPI.Modules.Orders.Domain.DTOs.Order;
using NordesteFoodAPI.Modules.Orders.Domain.DTOs.OrderItem;
using NordesteFoodAPI.Modules.Orders.Domain.Entities;
using NordesteFoodAPI.Modules.Orders.Domain.Enums;
using NordesteFoodAPI.Modules.Restaurants.Domain.Entities;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Orders.Application.UseCases
{
    public class GetOrdersByFilterUseCase
    {
        private IOrderRepository _orderRepository;

        public GetOrdersByFilterUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result<IEnumerable<OrderResponseDTO>>> GetByFiltersAsync(OrderFilterRequestDTO orderFilterRequestDTO)
        {
            var getOrdersResult = await _orderRepository.FindByFilterAsync(
                orderFilterRequestDTO.OrderChannel,
                orderFilterRequestDTO.OrderStatus,
                orderFilterRequestDTO.Page,
                orderFilterRequestDTO.Limit
            );

            if (!getOrdersResult.IsSuccess)
            {
                return Result<IEnumerable<OrderResponseDTO>>.Failure(
                    getOrdersResult.ErrorMessage ?? "Ocorreu um erro inesperado ao tentar buscar pedidos com os filtros informados.",
                    getOrdersResult.ErrorType
                );
            }

            var ordersResponseDTO = getOrdersResult.Value!.Select(order => new OrderResponseDTO(
                order.UserId,
                order.Id,
                order.RestaurantId,
                order.OrderStatus.ToString(),
                order.OrderChannel.ToString(),
                order.Total,
                order.RequestedAt,
                order.Items.Select(item => new OrderItemResponseDTO(
                    item.ProductId,
                    item.Quantity.Value,
                    item.UnitPrice,
                    item.Subtotal
                )).ToList()
            )).ToList();

            return Result<IEnumerable<OrderResponseDTO>>.Success(ordersResponseDTO);
        }
    }
}
