namespace StrategyManager.Core.Services.Abstractions
{
    public interface IStrategyFactory
    {
        IStrategy CreateStrategyByCode(string strategyCode);
    }
}
