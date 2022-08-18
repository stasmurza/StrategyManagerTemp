using StrategyManager.Core.Models.Services.Strategies.Turtles;
using StrategyManager.Core.Models.Store;
using StrategyManager.Core.Models.Store.Events;
using StrategyManager.Core.Repositories.Abstractions;
using System.Text.Json;

namespace StrategyManager.Core.Services.Strategies.Turtles
{
    public class PendingOrderCreator : IPendingOrderCreator
    {
        private IRepository<Event> eventRepository;
        private IRepository<Order> orderRepository;
        private readonly IUnitOfWork unitOfWork;

        public event EventHandler<PendingOrderEventArgs>? NewPendingOrder;
        public event EventHandler<EventArgs>? Started;
        public event EventHandler<EventArgs>? Stopped;

        public PendingOrderCreator(
            IRepository<Event> eventRepository,
            IRepository<Order> orderRepository,
            IUnitOfWork unitOfWork)
        {
            this.eventRepository = eventRepository;
            this.orderRepository = orderRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task CreatePendingOrderAsync(PendingOrderInput input)
        {
            if (Started != null) Started(this, EventArgs.Empty);

            var order = new Order
            {
                InstrumentCode = input.InstrumentCode,
                StrategyId = input.StrategyId,
                Direction = input.Direction,
                Volume = input.Volume,
                Price = input.Price,
                DateTime = DateTime.Now,
            };
            await orderRepository.AddAsync(order);

            var args = new PendingOrderEventArgs
            {
                InstrumentCode = input.InstrumentCode,
                StrategyId = input.StrategyId,
                Direction = input.Direction,
                Volume = input.Volume,
                Price = input.Price,
                DateTime = DateTime.Now,
            };
            var newEvent = new Event
            {
                EntityType = EntityType.TurtlesStrategy,
                EntityId = input.StrategyId,
                EventType = JsonSerializer.Serialize(EventType.NewEntrySignal),
                EventData = JsonSerializer.Serialize(args),
            };
            await eventRepository.AddAsync(newEvent);
            await unitOfWork.CompleteAsync();
            Stop();

            if (NewPendingOrder != null) NewPendingOrder(this, args);
        }

        public void Stop() { }
    }
}
