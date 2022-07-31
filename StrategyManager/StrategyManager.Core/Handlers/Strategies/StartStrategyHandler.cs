using StrategyManager.Core.Exceptions;
using StrategyManager.Core.Models.Store;
using StrategyManager.Core.Repositories.Abstractions;
using StrategyManager.Core.Services.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using StrategyManager.Core.Models.Handlers.Strategies;

namespace StrategyManager.Core.Handlers.Strategies
{
    public class StartStrategyHandler : IRequestHandler<StartStrategyInput, Unit> 
    {
        private readonly IRepository<Strategy> repository;
        private readonly IHostedServicePool hostedServicesPool;
        private readonly ILogger logger;

        public StartStrategyHandler(
            IRepository<Strategy> repository,
            IHostedServicePool hostedServicesPool,
            ILogger<StartStrategyHandler> logger)
        {
            this.repository = repository;
            this.hostedServicesPool = hostedServicesPool;
            this.logger = logger;
        }

        public async Task<Unit> Handle(StartStrategyInput input, CancellationToken cancellationToken)
        {
            var job = await repository.GetByIdAsync(input.Id);
            if (job == null)
            {
                var message = $"Job with id {input.Id} is not found";
                throw new NotFoundException(message)
                {
                    ErrorCode = ErrorCodes.NotFound
                };
            }

            var service = hostedServicesPool.GetHostedServiceByJobCode(job.Code);
            service.StartJob();
            logger.LogInformation($"Job {input.Id} started by command");

            return Unit.Value;
        }
    }
}
