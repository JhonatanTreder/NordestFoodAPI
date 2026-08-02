using NordesteFoodAPI.Modules.Feedbacks.Application.UseCases;
using NordesteFoodAPI.Modules.Feedbacks.Domain.Contracts.Repositories;
using NordesteFoodAPI.Modules.Feedbacks.Infreaestructure.Persistence.Repositories;

namespace NordesteFoodAPI.Modules.Feedbacks.Infreaestructure.DependencyInjection
{
    public static class FeedbacksModule
    {
        public static IServiceCollection AddFeedbacksModule(this IServiceCollection services)
        {
            services.AddScoped<IFeedbacksRepository, FeedbacksRepository>();
            services.AddScoped<CreateFeedbackUseCase>();

            return services;
        }
    }
}
