using StrategyManager.Core.Proxies.Clients;
using StrategyManager.Core.Services.Abstractions;
using StrategyManager.Infrastructure.Proxies.Clients;

namespace StrategyManager.WebAPI.DependencyInjection
{
    /// <summary>
    /// Infrastructure service injection
    /// </summary>
    public static class InfrastructureServiceInjection
    {
        /// <summary>
        /// Add infrastructure services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<ISmtpClient, SmtpClientWrapper>();
            services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();

            return services;
        }
    }
}
