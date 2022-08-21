using StrategyManager.Core.Models.Store;
using StrategyManager.Core.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StrategyManager.Core.Services.Strategies.Abstractions;

namespace StrategyManager.Core.Services
{
    /// <summary>
    /// For starting services on startup 
    /// </summary>
    public class HostedStrategyService : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IStrategyManager strategyManager;
        private readonly ILogger logger;

        public HostedStrategyService(
            IServiceProvider serviceProvider,
            IStrategyManager strategyManager,
            ILogger<HostedStrategyService> logger)
        {
            this.serviceProvider = serviceProvider;
            this.strategyManager = strategyManager;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.Register(async () => await StopStrategiesAsync(CancellationToken.None));
            await StartStrategiesAsync(cancellationToken);
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"{nameof(HostedStrategyService)} is starting.");
            await base.StartAsync(cancellationToken);
            logger.LogInformation($"{nameof(HostedStrategyService)} is running.");
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"{nameof(HostedStrategyService)} is stopping.");
            await StopStrategiesAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
            logger.LogInformation($"{nameof(HostedStrategyService)} is stopped.");
        }

        public async Task StartStrategiesAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"Strategies are starting.");
            using (var scope = serviceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetService<IRepository<Strategy>>();
                var message = $"Unsuccessful attempt to activate a service {nameof(IRepository<Strategy>)}";
                if (repository is null) throw new InvalidOperationException(message);

                var strategies = await repository.GetAsync();
                foreach(var strategy in strategies)
                {
                    if (!strategy.StartWithHost) continue;
                    foreach (var ticket in strategy.Tickets)
                    {
                        strategyManager.Start(strategy.Code, ticket.Code);
                        if (cancellationToken.IsCancellationRequested) return;
                    }
                }
            }
        }

        public async Task StopStrategiesAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"Strategies are stopping.");
            var strategies = strategyManager.GetStrategies();
            foreach (var strategy in strategies)
            {
                await strategyManager.StopAsync(strategy.StrategyCode.ToString(), strategy.TicketCode);
                if (cancellationToken.IsCancellationRequested) return;
            }
        }
    }
}
