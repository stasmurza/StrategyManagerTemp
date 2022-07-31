using AutoMapper;
using StrategyManager.Core.Models.DTOs;
using StrategyManager.Core.Models.Services;
using StrategyManager.Core.Models.Store;
using StrategyManager.Core.Repositories.Abstractions;
using StrategyManager.Core.Services.Abstractions;
using MediatR;
using StrategyManager.Core.Models.Handlers.Strategies;

namespace StrategyManager.Core.Handlers.Strategies
{
    public class GetActiveStrategiesHandler : IRequestHandler<GetActiveStrategiesInput, GetActiveStrategiesOutput> 
    {
        private readonly IStrategyManager strategyManager;
        private readonly IRepository<Strategy> repository;
        private readonly IStrategyPool jobsPool;
        private readonly IMapper mapper;

        public GetActiveStrategiesHandler(
            IRepository<Strategy> repository,
            IStrategyPool jobsPool,
            IMapper mapper)
        {
            this.repository = repository;
            this.jobsPool = jobsPool;
            this.mapper = mapper;
        }

        public async Task<GetActiveStrategiesOutput> Handle(GetActiveStrategiesInput input, CancellationToken cancellationToken)
        {
            var jobs = await strategyManager.GetTicketStatuses();
            var activeJobs = new List<Strategy>();
            foreach(var job in jobs)
            {
                var service = jobsPool.GetJobByCode(job.Code);
                if (service.Status == StrategyStatus.Running) activeJobs.Add(job);
            }

            return new GetActiveStrategiesOutput
            {
                Jobs = activeJobs.Select(i => mapper.Map(i, new StrategyDTO()))
            };
        }
    }
}
