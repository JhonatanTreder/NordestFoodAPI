using NordesteFoodAPI.Modules.UnitProducts.Application.UseCases;
using NordesteFoodAPI.Modules.UnitProducts.Domain.Contracts;
using NordesteFoodAPI.Modules.UnitProducts.Infraestructure.Persistence.Repositories;
using System.Runtime.CompilerServices;

namespace NordesteFoodAPI.Modules.UnitProducts.Infraestructure.DependencyInjection
{
    public static class UnitProductModule
    {
        public static IServiceCollection AddUnitProductModule(this IServiceCollection services)
        {
            services.AddScoped<IUnitProductRepository, UnitProductRepository>();
            services.AddScoped<CreateUnitProductUseCase>();
            services.AddScoped<GetRestaurantMenuUseCase>();

            return services;
        }
    }
}
