using StrategyManager.Core.Models.Services.Strategies;

namespace StrategyManager.Core.Models.Services.MarketDataProvider
{
    public class MarketDataEventArgs : EventArgs
    {
        public MarketDataEventArgs(MarketData marketData)
        {
            MarketData = marketData;
        }
        public MarketData MarketData { get; private set; }
    }
}
