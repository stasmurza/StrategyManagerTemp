using StrategyManager.Core.Models.Services;
using StrategyManager.Core.Models.Services.MarketDataProvider;

namespace StrategyManager.Core.Services.Abstractions
{
    public interface IMarketDataProvider : IDisposable
    {
        void Subscribe(string ticketCode, TimeFrame timeFrame);
        public event EventHandler<MarketDataEventArgs> PriceChanged;
    }
}
