using StrategyManager.Core.Models.Services.Strategies;

namespace StrategyManager.Core.Services.Abstractions.Strategies
{
    public interface IStrategyFactory
    {
        IStrategy CreateStrategyByCode(StrategyCode strategyCode, string ticketCode);
    }
}
