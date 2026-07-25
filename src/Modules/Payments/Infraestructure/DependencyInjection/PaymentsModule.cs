using NordesteFoodAPI.Modules.Payments.Application.UseCases;
using NordesteFoodAPI.Modules.Payments.Domain.Contracts.Repositories;
using NordesteFoodAPI.Modules.Payments.Domain.Contracts.Services;
using NordesteFoodAPI.Modules.Payments.Infraestructure.Repositories;
using NordesteFoodAPI.Modules.Payments.Infraestructure.Services;

namespace NordesteFoodAPI.Modules.Payments.Infraestructure.DependencyInjection
{
    public static class PaymentsModule
    {
        public static IServiceCollection AddPaymentsModule(this IServiceCollection services)
        {
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentService, PaymentServiceMock>();
            services.AddScoped<CreatePaymentUseCase>();

            return services;
        }
    }
}
