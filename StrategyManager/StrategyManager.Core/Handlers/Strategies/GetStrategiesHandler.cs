using AutoMapper;
using StrategyManager.Core.Models.Handlers.Strategies;
using StrategyManager.Core.Models.Store;
using StrategyManager.Core.Repositories.Abstractions;
using MediatR;
using StrategyManager.Core.Models.DTOs.Strategies;

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
            var strategies = await repository.GetAllAsync();

            return new GetStrategiesOutput
            {
                Strategies = strategies.Select(i => mapper.Map(i, new StrategyDTO()))
            };
        }
    }
}
