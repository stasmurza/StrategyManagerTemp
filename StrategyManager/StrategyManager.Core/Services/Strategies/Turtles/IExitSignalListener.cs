using StrategyManager.Core.Models.Services.Strategies.Turtles;
using StrategyManager.Core.Services.Abstractions.Strategies;

namespace StrategyManager.Core.Services.Strategies.Turtles
{
    public interface IExitSignalListener : IIdempotentCommand
    {
        public void Run(ExitSignalInput input);

        public event EventHandler<ExitSignalEventArgs>? ExitSignal;
    }
}
