using StrategyManager.Core.Models.Services.Strategies;
using StrategyManager.Core.Services.Abstractions;

namespace StrategyManager.Core.Services
{
    public class HistoryProvider : IHistoryProvider
    {
        public IEnumerable<MarketData> GetHistory(TimeFrame timeFrame, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
