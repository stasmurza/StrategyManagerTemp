using StrategyManager.Core.Models.Services;
using StrategyManager.Core.Models.Services.Strategies;

namespace StrategyManager.Core.Services.Abstractions.Strategies
{
    public interface IExitRules
    {
        /// <summary>
        /// Entry signal 
        /// </summary>
        public event EventHandler<ClosePositionEventArgs>? NewSignal;

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
