using StrategyManager.Core.Models.Store.Events;

namespace StrategyManager.Core.Services.Abstractions
{
    public interface IMessagePublisher
    {
        public void Publish(Event domainEvent);
    }
}
