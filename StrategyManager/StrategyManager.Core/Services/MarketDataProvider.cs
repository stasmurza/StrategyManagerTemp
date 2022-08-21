using StrategyManager.Core.Models.Services;
using StrategyManager.Core.Models.Services.MarketDataProvider;
using StrategyManager.Core.Services.Abstractions;

namespace StrategyManager.Core.Services
{
    public class MarketDataProvider : IMarketDataProvider
    {
        public event EventHandler<MarketDataEventArgs> PriceChanged;

        public void Subscribe(string ticketCode, TimeFrame timeFrame)
        {
        }

        public void Unsubscribe(string ticketCode, TimeFrame timeFrame)
        {
        }
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
