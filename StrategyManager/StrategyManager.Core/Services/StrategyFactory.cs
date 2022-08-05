using StrategyManager.Core.Models.Services.Strategies;
using StrategyManager.Core.Services.Abstractions;
using StrategyManager.Core.Services.Strategies;

namespace StrategyManager.Core.Services
{
    public class StrategyFactory : IStrategyFactory
    {
        public IStrategy CreateStrategyByCode(StrategyCode strategyCode, string ticketCode)
        {
            switch (strategyCode)
            {
                case StrategyCode.Turtles:
                {
                    return new TurtlesStrategy(strategyCode, ticketCode);
                }
                default: throw new ArgumentOutOfRangeException(nameof(strategyCode), $"Not expected value: {strategyCode}");
            };
        }
    }
}
