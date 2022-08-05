using MediatR;
using StrategyManager.Core.Services;
using StrategyManager.Core.Services.Abstractions;
using StrategyManager.Core.Services.Abstractions.Strategies;
using StrategyManager.Core.Services.Strategies;

namespace StrategyManager.WebAPI.DependencyInjection
{
    /// <summary>
    /// Business layer service injection
    /// </summary>
    public static class BusinessLayerServiceInjection
    {
        /// <summary>
        /// Add business layer services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBusinessLayerServices(this IServiceCollection services)
        {
            services.AddTransient<ITurtlesStrategy, TurtlesStrategy>();
            services.AddScoped<IStrategyFactory, StrategyFactory>();
            services.AddHostedService<HostedEventPublisher>();

            //For starting services on startup
            services.AddHostedService<HostedStrategyService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IJwtTokenService, JwtTokenService>();

            services.AddMediatR(typeof(StrategyManager.Core.Handlers.Strategies.RunStrategyHandler));

            return services;
        }
    }
}
