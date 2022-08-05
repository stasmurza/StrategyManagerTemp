using StrategyManager.Core.Models.Services.Strategies.Reports;

namespace StrategyManager.Core.Services.Abstractions
{
    public interface IStrategyManagerReports
    {
        public Func<IEnumerable<IStrategy>> GetStrategies { get; set; }

        public IEnumerable<TicketReport> TicketsReport();
    }
}
