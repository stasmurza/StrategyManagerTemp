using StrategyManager.Core.Exceptions;
using StrategyManager.Core.Models.Store;
using StrategyManager.Core.Repositories.Abstractions;
using StrategyManager.Core.Services.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using StrategyManager.Core.Models.Handlers.Strategies;

namespace StrategyManager.Core.Handlers.Strategies
{
    public class RunStrategyHandler : IRequestHandler<RunStrategyInput, Unit> 
    {
        private readonly IStrategyManager strategyManager;
        private readonly IRepository<Strategy> repository;
        private readonly ILogger logger;

        public RunStrategyHandler(
            IStrategyManager strategyManager,
            IRepository<Strategy> repository,
            ILogger<RunStrategyHandler> logger)
        {
            this.strategyManager = strategyManager;
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Unit> Handle(RunStrategyInput input, CancellationToken cancellationToken)
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

            foreach (var ticket in strategy.Tickets)
            {
                strategyManager.Start(strategy.Code, ticket.Code);
                if (cancellationToken.IsCancellationRequested) break;
                logger.LogInformation($"Strategy {input.Id} started by command");
            }

            return Unit.Value;
        }
    }
}
