using NordesteFoodAPI.Modules.Restaurants.Application.UseCases;
using NordesteFoodAPI.Modules.Restaurants.Domain.Contracts;
using NordesteFoodAPI.Modules.Restaurants.Infraestructure.Persistence.Repositories;

namespace NordesteFoodAPI.Modules.Restaurants.Infraestructure.DependencyInjection
{
    public static class RestaurantModule
    {
        public static IServiceCollection AddRestaurantModule(this IServiceCollection services)
        {
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            services.AddScoped<CreateRestaurantUseCase>();

            return services;
        }
    }
}
