using StrategyManager.Core.Models.Services.MarketDataProvider;
using StrategyManager.Core.Models.Services.Strategies;

namespace StrategyManager.Core.Services.Abstractions
{
    public interface IMarketDataProvider : IDisposable
    {
        void Subscribe(string ticketCode, TimeFrame timeFrame);
        public event EventHandler<MarketDataEventArgs> PriceChanged;
    }
}
