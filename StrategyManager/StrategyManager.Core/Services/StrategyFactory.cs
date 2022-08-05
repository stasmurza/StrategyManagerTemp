using Microsoft.Extensions.DependencyInjection;
using StrategyManager.Core.Models.Services.Strategies;
using StrategyManager.Core.Services.Abstractions;
using StrategyManager.Core.Services.Strategies;

namespace StrategyManager.Core.Services
{
    public class StrategyFactory : IStrategyFactory
    {
        private readonly IServiceProvider serviceProvider;

        public StrategyFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IStrategy CreateStrategyByCode(string strategycode)
        {
            var parsed = Enum.TryParse(strategycode, out StrategyCode result);
            if (!parsed) throw new ArgumentOutOfRangeException(nameof(strategycode), $"Not expected value: {strategycode}");

            switch (result)
            {
                case StrategyCode.Turtles:
                {
                    var scope = serviceProvider.CreateScope();
                    var strategy = scope.ServiceProvider.GetService<TurtlesStrategy>();
                    var message = $"Unsuccessful attempt to activate a service {nameof(TurtlesStrategy)}";
                    if (strategy is null) throw new InvalidOperationException(message);
                    return strategy;

                }
                default: throw new ArgumentOutOfRangeException(nameof(result), $"Not expected value: {result}");
            };
        }
    }
}
