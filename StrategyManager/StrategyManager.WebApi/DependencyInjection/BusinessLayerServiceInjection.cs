using MediatR;
using StrategyManager.Core.Services;
using StrategyManager.Core.Services.Abstractions;
using StrategyManager.Core.Services.Strategies.Abstractions;
using StrategyManager.Core.Services.Strategies.Turtles;
using StrategyManager.Core.Services.Strategies.Turtles.Abstractions;
using TradingAPI.API.Services;

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
            services.AddTransient<IMarketDataProvider, MarketDataProvider>();
            services.AddTransient<IHistoryProvider, HistoryProvider>();
            services.AddTransient<IOrderManager, OrderManager>();

            //Turtles
            services.AddTransient<IEntrySignalListener, EntrySignalListener>();
            services.AddTransient<IEntryOrderCreator, EntryOrderCreator>();
            services.AddTransient<IOrderHandler, OrderHandler>();
            services.AddTransient<IExitSignalListener, ExitSignalListener>();
            services.AddTransient<IExitOrderCreator, ExitOrderCreator>();
            services.AddTransient<IOrderHandler, OrderHandler>();
            services.AddTransient<ITurtlesStrategy, TurtlesStrategy>();

            services.AddSingleton<IStrategyFactory, StrategyFactory>();
            services.AddSingleton<IStrategyManager, Core.Services.StrategyManager>();
            services.AddHostedService<HostedEventPublisher>();

            //For starting services on startup
            services.AddHostedService<HostedStrategyService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IJwtTokenService, JwtTokenService>();

            services.AddMediatR(typeof(Core.Handlers.Strategies.RunStrategyHandler));

            return services;
        }
    }
}
