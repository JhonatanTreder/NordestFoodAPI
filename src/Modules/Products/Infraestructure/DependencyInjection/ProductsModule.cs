using NordesteFoodAPI.Modules.Products.Application.UseCases;
using NordesteFoodAPI.Modules.Products.Domain.Contracts;
using NordesteFoodAPI.Modules.Products.Infraestructure.Persistence.Repositories;

namespace NordesteFoodAPI.Modules.Products.Infraestructure.DependencyInjection
{
    public static class ProductsModule 
    {
        public static IServiceCollection AddProductsModule(this IServiceCollection services)
        {
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<CreateProductUseCase>();

            return services;
        }
    }
}
