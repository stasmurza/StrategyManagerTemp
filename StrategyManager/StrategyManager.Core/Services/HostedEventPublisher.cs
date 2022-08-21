using StrategyManager.Core.Models.Store.Events;
using StrategyManager.Core.Repositories.Abstractions;
using StrategyManager.Core.Services.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace StrategyManager.Core.Services
{
    public class HostedEventPublisher : BackgroundService
    {
        private readonly IMessagePublisher messagePublisher;
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger logger;
        private CancellationTokenSource cancellationTokenSource = new();

        public HostedEventPublisher(
            IMessagePublisher messagePublisher,
            IServiceProvider serviceProvider,
            ILogger<HostedEventPublisher> logger)
        {
            this.serviceProvider = serviceProvider;
            this.messagePublisher = messagePublisher;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var task = Task.Run(async () => await ProcessEvents(cancellationToken));
            var continueTask = task.ContinueWith((t) =>
            {
                if (t.Exception != null)
                {
                    logger.LogError(t.Exception.Message);
                    throw t.Exception;
                }
            });
            GC.KeepAlive(task);
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"{nameof(HostedEventPublisher)} is starting.");
            await base.StartAsync(cancellationToken);
            logger.LogInformation($"{nameof(HostedEventPublisher)} is running.");
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"{nameof(HostedEventPublisher)} is stopping.");
            await base.StopAsync(cancellationToken);
            logger.LogInformation($"{nameof(HostedEventPublisher)} has stopped.");
        }

        private async Task ProcessEvents(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;

            using (var scope = serviceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetService<IRepository<Event>>();
                var message = $"Unsuccessful attempt to activate a service {nameof(IRepository<Event>)}";
                if (repository is null) throw new InvalidOperationException(message);
                var newEvents = await repository.GetAsync(i => i.Published == false);

                //publish events to message queue
                foreach (var domainEvent in newEvents)
                {
                    if (cancellationToken.IsCancellationRequested) return;
                    messagePublisher.Publish(domainEvent);
                    domainEvent.Published = true;
                    repository.Update(domainEvent);
                }
            }
            
            await Task.Delay(1000);
            await ProcessEvents(cancellationToken);
        }
    }
}
