using AutoMapper;
using StrategyManager.Core.Models.Store;
using StrategyManager.Core.Repositories.Abstractions;
using StrategyManager.Core.Services.Abstractions;
using MediatR;
using StrategyManager.Core.Models.Handlers.StrategiesReport;
using StrategyManager.Core.Models.DTOs.Reports;

namespace StrategyManager.Core.Handlers.StrategiesReports
{
    public class ActiveStrategiesReportHandler : IRequestHandler<ActiveStrategiesReportInput, ActiveStrategiesReportOutput> 
    {
        private readonly IStrategyManager strategyManager;
        private readonly IRepository<Strategy> repository;
        private readonly IStrategyFactory strategyPool;
        private readonly IMapper mapper;

        public ActiveStrategiesReportHandler(
            IStrategyManager strategyManager,
            IRepository<Strategy> repository,
            IStrategyFactory strategyPool,
            IMapper mapper)
        {
            this.strategyManager = strategyManager;
            this.repository = repository;
            this.strategyPool = strategyPool;
            this.mapper = mapper;
        }

        public async Task<ActiveStrategiesReportOutput> Handle(ActiveStrategiesReportInput input, CancellationToken cancellationToken)
        {
            var strategies = strategyManager.GetStrategies();
            var groups = strategies
                .GroupBy(i => i.StrategyCode);

            var repositoryStrategies = await repository.GetAllAsync();
            var list = new List<StrategyDTO>();
            foreach (var group in groups)
            {
                var strategy = repositoryStrategies.FirstOrDefault(i => i.Code == group.Key.ToString());
                if (strategy is null)
                {
                    var message = $"Strategy with code {group.Key} is not found";
                    throw new InvalidOperationException(message);
                }

                var activeStrategy = mapper.Map(strategy, new StrategyDTO());
                activeStrategy.Tickets.Clear();

                foreach (var item in group)
                {
                    activeStrategy.Tickets.Add(new TicketDTO
                    {
                        Code = item.TicketCode,
                        Status = item.Status.ToString(),
                    });
                }

                list.Add(activeStrategy);
            }

            return new ActiveStrategiesReportOutput
            {
                Strategies = list
            };
        }
    }
}
