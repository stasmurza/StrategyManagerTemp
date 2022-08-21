using Microsoft.Extensions.DependencyInjection;
using StrategyManager.Core.Models.Options;
using StrategyManager.Core.Models.Services.Strategies;
using StrategyManager.Core.Services.Strategies;
using StrategyManager.Core.Services.Strategies.Abstractions;
using StrategyManager.Core.Services.Strategies.Turtles.Abstractions;

namespace StrategyManager.Core.Services
{
    public class StrategyFactory : IStrategyFactory
    {
        private readonly IServiceProvider serviceProvider;
        public StrategyFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IStrategy CreateStrategyByCode(StrategyCode strategyCode)
        {
            switch (strategyCode)
            {
                case StrategyCode.Turtles:
                {
                        var strategy = serviceProvider.GetService<ITurtlesStrategy>();
                        var message = $"Unsuccessful attempt to activate a service {nameof(ITurtlesStrategy)}";
                        if (strategy is null) throw new InvalidOperationException(message);
                        return strategy;
                }
                default: throw new ArgumentOutOfRangeException(nameof(strategyCode), $"Not expected value: {strategyCode}");
            };
        }
    }
}
