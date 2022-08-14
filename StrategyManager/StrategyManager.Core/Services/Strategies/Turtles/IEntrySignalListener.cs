using StrategyManager.Core.Models.Services.Strategies.Turtles;
using StrategyManager.Core.Services.Abstractions.Strategies;

namespace StrategyManager.Core.Services.Strategies.Turtles
{
    public interface IEntrySignalListener : IIdempotentCommand
    {
        public void Run(EntrySignalInput input);

        public event EventHandler<EntrySignalEventArgs>? EntrySignal;
    }
}
