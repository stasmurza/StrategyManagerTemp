using StrategyManager.Core.Models.Services.Strategies.Turtles;
using StrategyManager.Core.Services.Strategies.Abstractions;

namespace StrategyManager.Core.Services.Strategies.Turtles.Abstractions
{
    public interface IExitSignalListener : IIdempotentStep
    {
        public void Run(ExitSignalInput input);

        public event EventHandler<ExitSignalEventArgs>? ExitSignal;
    }
}
