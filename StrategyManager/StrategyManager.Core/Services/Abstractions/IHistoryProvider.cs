using StrategyManager.Core.Models.Services;

namespace StrategyManager.Core.Services.Abstractions
{
    public interface IHistoryProvider : IDisposable
    {
        IEnumerable<MarketData> GetHistory(TimeFrame timeFrame, DateTime startDate, DateTime endDate);
    }
}
