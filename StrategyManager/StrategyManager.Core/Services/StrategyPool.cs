using StrategyManager.Core.Models.Services.Jobs;
using StrategyManager.Core.Models.Services.Strategies;
using StrategyManager.Core.Services.Abstractions;
using StrategyManager.Core.Services.Strategies;

namespace StrategyManager.Core.Services
{
    public class StrategyPool : IStrategyPool
    {
        private readonly IStrategy turtlesStrategy;
        public StrategyPool(TurtlesStrategy turtlesStrategy)
        {
            this.turtlesStrategy = turtlesStrategy;
        }

        public IStrategy GetStrategyByCode(string id)
        {
            var parsed = Enum.TryParse(id, out StrategyCode result);
            if (!parsed) throw new ArgumentOutOfRangeException(nameof(id), $"Not expected value: {id}");

            return result switch
            {
                StrategyCode.Turtles => turtlesStrategy,
                _ => throw new ArgumentOutOfRangeException(nameof(result), $"Not expected value: {result}"),
            };
        }
    }
}
