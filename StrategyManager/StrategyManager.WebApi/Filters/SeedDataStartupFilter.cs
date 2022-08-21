using StrategyManager.Core.Repositories.Abstractions;
using StrategyManager.Data;
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
                var dbContext = scope.ServiceProvider.GetService<StrategyManagerDbContext>();
                var message = $"Unsuccessful attempt to activate a service {nameof(StrategyManagerDbContext)}";
                if (dbContext is null) throw new InvalidOperationException(message);

                var strategyRepoitory = scope.ServiceProvider.GetService<IRepository<Core.Models.Store.Strategy>>();
                message = $"Unsuccessful attempt to activate a service {nameof(IRepository<Core.Models.Store.Strategy>)}";
                if (strategyRepoitory is null) throw new InvalidOperationException(message);

                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
                message = $"Unsuccessful attempt to activate a service {nameof(IUnitOfWork)}";
                if (unitOfWork is null) throw new InvalidOperationException(message);

                //Strategies
                var turtles = strategyRepoitory
                    .FirstOrDefaultAsync(i => i.Code == StrategyCode.Turtles.ToString())
                    .Result;

                if (turtles == null)
                {
                    turtles = new Core.Models.Store.Strategy
                    {
                        Code = StrategyCode.Turtles.ToString(),
                        Name = StrategyCode.Turtles.ToString(),
                        StartWithHost = false,
                    };
                    strategyRepoitory
                        .AddAsync(turtles)
                        .Wait();

                    unitOfWork.CompleteAsync().Wait();
                }
                
            }
        }
    }
}
