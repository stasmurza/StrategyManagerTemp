using StrategyManager.Core.Models.Services.Strategies.Turtles;
using StrategyManager.Core.Services.Abstractions.Strategies;

namespace StrategyManager.Core.Services.Strategies.Turtles
{
    public interface IPendingOrderCreator : IIdempotentCommand
    {
        public void CreatePendingOrder(PendingOrderInput input);

        public event EventHandler<PendingOrderEventArgs>? NewPendingOrder;
    }
}
