using StrategyManager.Core.Models.Services.Strategies.Turtles;
using StrategyManager.Core.Services.Strategies.Abstractions;

namespace StrategyManager.Core.Services.Strategies.Turtles.Abstractions
{
    public interface IPendingOrderCreator : IIdempotentCommand
    {
        public Task CreatePendingOrderAsync(PendingOrderInput input);

        public event EventHandler<PendingOrderEventArgs>? NewPendingOrder;
    }
}
