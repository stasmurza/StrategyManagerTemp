using StrategyManager.Core.Exceptions;
using StrategyManager.Core.Models.Store;
using StrategyManager.Core.Repositories.Abstractions;
using StrategyManager.Core.Services.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using StrategyManager.Core.Models.Handlers.Strategies;

namespace StrategyManager.Core.Handlers.Strategies
{
    public class StopStrategyHandler : IRequestHandler<StopStrategyInput, Unit> 
    {
        private readonly IRepository<Strategy> repository;
        private readonly IHostedServicePool hostedServicesPool;
        private readonly ILogger logger;

        public StopStrategyHandler(
            IRepository<Strategy> repository,
            IHostedServicePool hostedServicesPool,
            ILogger<StopStrategyHandler> logger)
        {
            this.repository = repository;
            this.hostedServicesPool = hostedServicesPool;
            this.logger = logger;
        }

        public async Task<Unit> Handle(StopStrategyInput input, CancellationToken cancellationToken)
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
            await service.StopJobAsync();
            logger.LogInformation($"Job {input.Id} stopped by command");

            return Unit.Value;
        }
    }
}
