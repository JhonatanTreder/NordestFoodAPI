using NordesteFoodAPI.Modules.Orders.Domain.Contracts;
using NordesteFoodAPI.Modules.Orders.Domain.DTOs.Order;
using NordesteFoodAPI.Modules.Orders.Domain.DTOs.OrderItem;
using NordesteFoodAPI.Modules.Orders.Domain.Entities;
using NordesteFoodAPI.Modules.Orders.Domain.Enums;
using NordesteFoodAPI.Modules.Restaurants.Domain.Contracts;
using NordesteFoodAPI.Modules.UnitProducts.Domain.Contracts;
using NordesteFoodAPI.Modules.UnitProducts.Domain.Entities;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Orders.Application.UseCases
{
    public class CreateOrderUseCase
    {
        private readonly  IOrderRepository _orderRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IUnitProductRepository _unitProductRepository;

        public CreateOrderUseCase(
            IOrderRepository orderRepository,
            IRestaurantRepository restaurantRepository,
            IUnitProductRepository unitProductRepository)
        {
            _orderRepository = orderRepository;
            _restaurantRepository = restaurantRepository;
            _unitProductRepository = unitProductRepository;
        }

        public async Task<Result<OrderResponseDTO>> CreateAsync(CreateOrderRequestDTO orderRequestDTO, Guid userId)
        {
            var conversioResult = Enum.TryParse<OrderChannel>(orderRequestDTO.OrderChannel, true, out var orderChannel);

            if (conversioResult is false)
            {
                return Result<OrderResponseDTO>.Failure("Não foi possível realizar a conversão do canal do pedido.", ErrorType.ValidationError);
            }

            var resturantExists = await _restaurantRepository.FindByIdAsync(orderRequestDTO.RestaurantId);

            if (resturantExists is null)
            {
                return Result<OrderResponseDTO>.Failure("O restaurante não foi encontrado.", ErrorType.NotFound);
            }

            if (orderRequestDTO.Items is null || orderRequestDTO.Items.Count == 0)
            {
                return Result<OrderResponseDTO>.Failure(
                    "Um pedido deve possuir pelo menos um item para ser criado.",
                    ErrorType.ValidationError
                );
            }

            var repositoryResponse = await _unitProductRepository.FindByRestaurantIdAsync(orderRequestDTO.RestaurantId);

            if (!repositoryResponse.IsSuccess)
            {
                return Result<OrderResponseDTO>.Failure(
                    $"Ocorreu um erro inesperado ao buscar pelo menu do restaurante de Id {orderRequestDTO.RestaurantId}",
                    ErrorType.UnexpectedFailure
                );
            }

            var restaurantMenu = repositoryResponse.Value;

            if (restaurantMenu is null)
            {
                return Result<OrderResponseDTO>.Failure("O cardápio do restaurante não foi encontrado.", ErrorType.NotFound);
            }

            var productsById = restaurantMenu.ToDictionary(p => p.ProductId);

            var order = Order.Create(
                userId,
                orderRequestDTO.RestaurantId,
                orderChannel
            );

            foreach (OrderItemRequestDTO item in orderRequestDTO.Items)
            {
                if (!productsById.ContainsKey(item.ProductId))
                {
                    return Result<OrderResponseDTO>.Failure(
                        $"O produto de Id '{item.ProductId}' solicitado pelo cliente não existe",
                        ErrorType.NotFound
                    );
                }

                var unitProduct = productsById[item.ProductId];

                if (!unitProduct.IsAvailable)
                {
                    return Result<OrderResponseDTO>.Failure(
                        $"O produto de Id '{item.ProductId}' solicitado pelo cliente não está disponível",
                        ErrorType.Conflict
                    );
                }

                order.AddItem(
                    item.ProductId,
                    item.Quantity,
                    unitProduct.Price.Value
                );
            }

            if (order.Items.Count == 0)
            {
                return Result<OrderResponseDTO>.Failure(
                    "Um pedido deve possuir pelo menos um item para ser criado.",
                    ErrorType.Failure
                );
            }

            var saveResult = await _orderRepository.CreateAsync(order);

            if (!saveResult.IsSuccess)
            {
                return Result<OrderResponseDTO>.Failure(
                    "Ocorreu um erro inesperado ao tentar criar o pedido.",
                    ErrorType.UnexpectedFailure
                );
            }

            var orderResponseDTO = new OrderResponseDTO(
                userId,
                order.Id,
                orderRequestDTO.RestaurantId,
                OrderStatus.AguardandoPagamento.ToString(),
                orderChannel.ToString(),
                order.Total,
                DateTime.UtcNow,
                order.Items.Select(i => new OrderItemResponseDTO(
                    i.ProductId,
                    i.Quantity.Value,
                    i.UnitPrice,
                    i.Subtotal
                )).ToList()
            );

            return Result<OrderResponseDTO>.Success(orderResponseDTO);
        }
    }
}
