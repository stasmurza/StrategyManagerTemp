using StrategyManager.Core.Models.Services;
using StrategyManager.Core.Models.Services.Strategies;

namespace StrategyManager.Core.Services.Abstractions.Strategies
{
    public interface IEntryRules
    {
        /// <summary>
        /// Entry signal 
        /// </summary>
        public event EventHandler<NewPositionEventArgs>? NewSignal;

        /// <summary>
        /// Build state of strategy
        /// </summary>
        public void BuildState();

        /// <summary>
        /// Process changes of price
        /// </summary>
        public void Process(MarketData marketData, Position? activePosition);
    }
}
