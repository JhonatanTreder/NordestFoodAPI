using NordesteFoodAPI.Modules.Auth.Application.UseCases;
using NordesteFoodAPI.Modules.Auth.Domain.Contracts;
using NordesteFoodAPI.Modules.Auth.Infraestructure.Services;

namespace NordesteFoodAPI.Modules.Auth.Infraestructure.DependencyInjection
{
    public static class AuthModule
    {
        public static IServiceCollection AddAuthModule(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<RegisterUseCase>();
            services.AddScoped<LoginUseCase>();

            return services;
        }
    }
}
