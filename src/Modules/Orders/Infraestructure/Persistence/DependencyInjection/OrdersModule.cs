using NordesteFoodAPI.Modules.Orders.Application.UseCases;
using NordesteFoodAPI.Modules.Orders.Domain.Contracts;
using NordesteFoodAPI.Modules.Orders.Infraestructure.Persistence.Repositories;
using System.Runtime.CompilerServices;

namespace NordesteFoodAPI.Modules.Orders.Infraestructure.Persistence.DependencyInjection
{
    public static class OrdersModule
    {
        public static IServiceCollection AddOrderModule(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<CreateOrderUseCase>();
            services.AddScoped<GetOrderByIdUseCase>();

            return services;
        }
    }
}
