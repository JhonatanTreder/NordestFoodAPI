using NordesteFoodAPI.Modules.Stocks.Application.UseCases;
using NordesteFoodAPI.Modules.Stocks.Domain.Contracts;
using NordesteFoodAPI.Modules.Stocks.Infraestructure.Persistence.Repositories;

namespace NordesteFoodAPI.Modules.Stocks.Infraestructure.DependencyInjection
{
    public static class StocksModule
    {
        public static IServiceCollection AddStocksModule(this IServiceCollection services)
        {
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<IncreaseStockUseCase>();
            services.AddScoped<DecreaseStockUseCase>();

            return services;
        }
    }
}
