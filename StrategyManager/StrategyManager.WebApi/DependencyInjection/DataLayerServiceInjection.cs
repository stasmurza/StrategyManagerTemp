using StrategyManager.Core.Models.Store;
using StrategyManager.Core.Models.Store.Events;
using StrategyManager.Core.Repositories.Abstractions;
using StrategyManager.Data;
using StrategyManager.Data.Repositories;

namespace StrategyManager.WebAPI.DependencyInjection
{
    /// <summary>
    /// Repository injection
    /// </summary>
    public static class DataLayerServiceInjection
    {
        /// <summary>
        /// Add repositories
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IEventStoreDbContext, EventStoreDbContext>();
            services.AddSingleton<IRepository<Event>, EventRepository>();
            services.AddScoped<IRepository<Strategy>, JobRepository>();
            return services;
        }
    }
}
