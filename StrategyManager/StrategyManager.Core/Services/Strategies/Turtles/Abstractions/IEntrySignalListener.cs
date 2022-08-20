using StrategyManager.Core.Models.Services.Strategies.Turtles;
using StrategyManager.Core.Services.Strategies.Abstractions;

namespace StrategyManager.Core.Services.Strategies.Turtles.Abstractions
{
    public interface IEntrySignalListener : IIdempotentStep
    {
        public void Run(EntrySignalInput input);

        public event EventHandler<EntrySignalEventArgs>? EntrySignal;
    }
}
