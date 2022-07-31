using AutoMapper;
using StrategyManager.Core.Models.DTOs;
using StrategyManager.Core.Models.Handlers.Strategies;
using StrategyManager.Core.Models.Store;
using StrategyManager.Core.Repositories.Abstractions;
using MediatR;

namespace StrategyManager.Core.Handlers.Strategies
{
    public class GetStrategiesHandler : IRequestHandler<GetStrategiesInput, GetStrategiesOutput> 
    {
        private readonly IRepository<Strategy> repository;
        private readonly IMapper mapper;

        public GetStrategiesHandler(
            IRepository<Strategy> repository,
            IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<GetStrategiesOutput> Handle(GetStrategiesInput input, CancellationToken cancellationToken)
        {
            var jobs = await repository.GetAllAsync();

            return new GetStrategiesOutput
            {
                Jobs = jobs.Select(i => mapper.Map(i, new StrategyDTO()))
            };
        }
    }
}
