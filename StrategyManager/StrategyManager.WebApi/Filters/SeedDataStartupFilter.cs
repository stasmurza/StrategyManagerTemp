using StrategyManager.Core.Repositories.Abstractions;
using StrategyManager.Data;
using MongoDB.Driver;
using StrategyManager.Core.Models.Services.Strategies;

namespace StrategyManager.WebAPI.Filters
{
    /// <summary>
    /// Filter seeds data to database during application startup
    /// </summary>
    public class SeedDataStartupFilter : IStartupFilter
    {
        /// <summary>
        /// Seed data to database
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                SeedData(app.ApplicationServices);
                next(app);
            };
        }

        private void SeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<IEventStoreDbContext>();
                var message = $"Unsuccessful attempt to activate a service {nameof(IEventStoreDbContext)}";
                if (dbContext is null) throw new InvalidOperationException(message);

                var strategyRepoitory = scope.ServiceProvider.GetService<IRepository<Core.Models.Store.Strategy>>();
                message = $"Unsuccessful attempt to activate a service {nameof(IRepository<Core.Models.Store.Strategy>)}";
                if (strategyRepoitory is null) throw new InvalidOperationException(message);

                var collections = dbContext.Database.ListCollectionNames().ToList() ?? new List<string>();

                //Jobs
                if (!collections.Any(i => String.Equals(i, CollectionNames.Jobs)))
                {
                    dbContext.Database.CreateCollection(CollectionNames.Jobs);
                    
                    //Binance
                    var binanceJob = new Core.Models.Store.Strategy
                    {
                        Code = StrategyCode.Turtles.ToString(),
                        Name = "Turtles",
                        StartWithHost = false,
                    };
                    var task = strategyRepoitory.CreateAsync(binanceJob);
                    task.Wait();
                }
                
            }
        }
    }
}
