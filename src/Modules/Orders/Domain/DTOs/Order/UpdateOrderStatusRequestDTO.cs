using NordesteFoodAPI.Modules.Orders.Domain.Enums;

namespace NordesteFoodAPI.Modules.Orders.Domain.DTOs.Order
{
    public record UpdateOrderStatusRequestDTO(string OrderStatus);
}
