using StrategyManager.Core.Services;
using StrategyManager.Core.Services.Abstractions;
using StrategyManager.Core.Services.Binance;

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
            services.AddSingleton<IBinanceJob, BinanceDataCollector>();
            services.AddScoped<IStrategyPool, StrategyPool>();
            services.AddScoped<IHostedServicePool, HostedServicePool>();
            services.AddHostedService<HostedEventPublisher>();
            services.AddHostedService<HostedStrategyService<IBinanceJob>>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IJwtTokenService, JwtTokenService>();

            return services;
        }
    }
}
