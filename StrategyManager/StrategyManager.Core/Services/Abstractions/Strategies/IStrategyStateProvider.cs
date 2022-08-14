namespace StrategyManager.Core.Services.Abstractions.Strategies
{
    public interface IStrategyStateProvider
    {
        public string SetState();
        public string GetState();
    }
}
