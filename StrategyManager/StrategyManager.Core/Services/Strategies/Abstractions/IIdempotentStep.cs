using StrategyManager.Core.Models.Services.Strategies;

namespace StrategyManager.Core.Services.Strategies.Abstractions
{
    public interface IIdempotentStep
    {
        public StrategyStatus Status { get; }
        public void Stop();
        public event EventHandler<NewStatusEventArgs>? OnStatusChange;
    }
}
