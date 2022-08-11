using StrategyManager.Core.Models.Services.Strategies;
using StrategyManager.Core.Services.Strategies.Turtles;

namespace StrategyManager.Core.Services.Abstractions.Strategies
{
    public interface IStrategyStateProvider
    {
        public string GetState();
    }
}
