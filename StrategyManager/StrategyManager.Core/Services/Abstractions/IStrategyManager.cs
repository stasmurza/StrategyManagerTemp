using StrategyManager.Core.Models.Services.Strategies;

namespace StrategyManager.Core.Services.Abstractions
{
    public interface IStrategyManager
    {
        IEnumerable<Strategy> GetStrategies();
        void Start(string strategyCode, string ticketCode);
        Task StopAsync(string strategyCode, string ticketCode);
        public IStrategyManagerReports Reports { get; }
    }
}
