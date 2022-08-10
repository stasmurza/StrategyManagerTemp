using StrategyManager.Core.Models.Services.Strategies;

namespace StrategyManager.Core.Services.Abstractions.Strategies
{
    public interface IStrategyStateProvider
    {
        public StrategyState GetState();
    }
}
