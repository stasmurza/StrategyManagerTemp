using StrategyManager.Core.Models.Services;
using StrategyManager.Core.Services.Abstractions;

namespace StrategyManager.Core.Services
{
    public class HistoryProvider : IHistoryProvider
    {
        public IEnumerable<MarketData> GetHistory(string instrumentCode, TimeFrame timeFrame, DateTime startDate, DateTime endDate)
        {
            return Enumerable.Empty<MarketData>();
        }
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
