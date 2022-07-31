using StrategyManager.Core.Models.Services.Jobs;
using StrategyManager.Core.Models.Store;
using StrategyManager.Core.Repositories.Abstractions;
using StrategyManager.Data;
using MongoDB.Driver;

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

                var jobRepoitory = scope.ServiceProvider.GetService<IRepository<Strategy>>();
                message = $"Unsuccessful attempt to activate a service {nameof(IRepository<Strategy>)}";
                if (jobRepoitory is null) throw new InvalidOperationException(message);

                var collections = dbContext.Database.ListCollectionNames().ToList() ?? new List<string>();

                //Jobs
                if (!collections.Any(i => String.Equals(i, CollectionNames.Jobs)))
                {
                    dbContext.Database.CreateCollection(CollectionNames.Jobs);
                    
                    //Binance
                    var binanceJob = new Strategy
                    {
                        Code = JobCode.Binance.ToString(),
                        Name = "Binance API",
                        StartWithHost = false,
                    };
                    var task = jobRepoitory.CreateAsync(binanceJob);
                    task.Wait();
                }
                
            }
        }
    }
}
