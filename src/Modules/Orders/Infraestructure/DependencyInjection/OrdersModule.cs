using NordesteFoodAPI.Modules.Orders.Application.UseCases;
using NordesteFoodAPI.Modules.Orders.Domain.Contracts.Repositories;
using NordesteFoodAPI.Modules.Orders.Infraestructure.Persistence.Repositories;

namespace NordesteFoodAPI.Modules.Orders.Infraestructure.DependencyInjection
{
    public static class OrdersModule
    {
        public static IServiceCollection AddOrdersModule(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<CreateOrderUseCase>();
            services.AddScoped<GetOrderByIdUseCase>();
            services.AddScoped<StartOrderPreparationUseCase>();
            services.AddScoped<MarkAsReadyUseCase>();
            services.AddScoped<MarkAsCanceledUseCase>();
            services.AddScoped<MarkAsDeliveredUseCase>();

            return services;
        }
    }
}
