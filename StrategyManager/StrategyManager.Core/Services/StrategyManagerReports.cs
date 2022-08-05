using StrategyManager.Core.Exceptions;
using StrategyManager.Core.Models.Services.Strategies;
using StrategyManager.Core.Models.Services.Strategies.Reports;
using StrategyManager.Core.Services.Abstractions;

namespace StrategyManager.Core.Services
{
    public class StrategyManagerReports : IStrategyManagerReports
    {
        public Func<IEnumerable<IStrategy>> GetStrategies { get; set; } = new(() => Enumerable.Empty<IStrategy>());

        public TicketReport TicketReport(StrategyCode strategyCode, string ticketCode)
        {
            var key = strategyCode + ticketCode;
            var list = GetStrategies();
            var strategy = list.FirstOrDefault(i => i.Code == strategyCode && i.TicketCode == ticketCode);

            if (strategy is null)
            {
                var message = $"Strategy {strategyCode} with ticket {ticketCode} is not found";
                throw new NotFoundException(message);
            }

            return new TicketReport
            {
                StrategyCode = strategy.Code.ToString(),
                TicketCode = strategy.TicketCode,
                Status = strategy.Status,
                LastActive = strategy.LastActive,
            };
        }

        public IEnumerable<TicketReport> TicketsReport()
        {
            var list = GetStrategies();
            foreach (var strategy in list)
            {
                yield return new TicketReport
                {
                    StrategyCode = strategy.Code.ToString(),
                    TicketCode = strategy.TicketCode,
                    Status = strategy.Status,
                    LastActive = strategy.LastActive,
                };
            };
        }
    }
}
