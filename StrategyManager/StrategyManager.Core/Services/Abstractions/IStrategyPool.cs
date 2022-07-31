namespace StrategyManager.Core.Services.Abstractions
{
    public interface IStrategyPool
    {
        IStrategy GetStrategyByCode(string code);
    }
}
