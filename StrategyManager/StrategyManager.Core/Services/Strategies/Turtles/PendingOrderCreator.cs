using StrategyManager.Core.Models.Services.Strategies.Turtles;
using StrategyManager.Core.Models.Store.Events;
using StrategyManager.Core.Repositories.Abstractions;

namespace StrategyManager.Core.Services.Strategies.Turtles
{
    public class PendingOrderCreator : IPendingOrderCreator
    {
        private IRepository<Event> eventRepository;

        public void CreatePendingOrder(PendingOrderInput input)
        {
            throw new NotImplementedException();
        }
    }
}
