using StrategyManager.Core.Exceptions;
using StrategyManager.Core.Models.Store;
using StrategyManager.Core.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using StrategyManager.Core.Models.Handlers.Strategies;
using StrategyManager.Core.Services.Abstractions.Strategies;

namespace StrategyManager.Core.Handlers.Strategies
{
    public class StopStrategyHandler : IRequestHandler<StopStrategyInput, Unit> 
    {
        private readonly IStrategyManager strategyManager;
        private readonly IRepository<Strategy> repository;
        private readonly ILogger logger;

        public StopStrategyHandler(
            IStrategyManager strategyManager,
            IRepository<Strategy> repository,
            ILogger<StopStrategyHandler> logger)
        {
            this.strategyManager = strategyManager;
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Unit> Handle(StopStrategyInput input, CancellationToken cancellationToken)
        {
            var strategy = await repository.GetByIdAsync(input.Id);
            if (strategy == null)
            {
                var message = $"Strategy with id {input.Id} is not found";
                throw new NotFoundException(message)
                {
                    ErrorCode = ErrorCodes.NotFound
                };
            }

            var strategies = strategyManager
                .GetStrategies()
                .Where(i => i.StrategyCode.ToString() == strategy.Code);

            foreach (var item in strategies)
            {
                await strategyManager.StopAsync(item.StrategyCode.ToString(), item.TicketCode);
                if (cancellationToken.IsCancellationRequested) break;
                logger.LogInformation($"Strategy {item.StrategyCode} by ticket {item.TicketCode} stopped by command");
            }

            return Unit.Value;
        }
    }
}
