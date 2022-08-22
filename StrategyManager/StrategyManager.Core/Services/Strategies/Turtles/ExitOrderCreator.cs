using AutoMapper;
using Microsoft.Extensions.Logging;
using StrategyManager.Core.Models.Services.Strategies;
using StrategyManager.Core.Models.Services.Strategies.Turtles;
using StrategyManager.Core.Models.Store;
using StrategyManager.Core.Models.Store.Events;
using StrategyManager.Core.Repositories.Abstractions;
using StrategyManager.Core.Services.Strategies.Turtles.Abstractions;
using System.Text.Json;

namespace StrategyManager.Core.Services.Strategies.Turtles
{
    public class ExitOrderCreator : IExitOrderCreator
    {
        public StrategyStatus Status { get; private set; }
        public event EventHandler<PendingOrderEventArgs>? NewPendingOrder;
        public event EventHandler<NewStatusEventArgs>? OnStatusChange;

        private IRepository<Event> eventRepository;
        private IRepository<Order> orderRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<ExitOrderCreator> logger;
        private string StrategyId { get; set; } = String.Empty;

        public ExitOrderCreator(
            IRepository<Event> eventRepository,
            IRepository<Order> orderRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ExitOrderCreator> logger)
        {
            this.eventRepository = eventRepository;
            this.orderRepository = orderRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
            OnStatusChange += ExitOrderCreator_OnStatusChange;
        }

        public async Task CreatePendingOrderAsync(PendingOrderInput input)
        {
            if (OnStatusChange != null)
            {
                OnStatusChange(this, new NewStatusEventArgs(StrategyStatus.Starting));
                OnStatusChange(this, new NewStatusEventArgs(StrategyStatus.Running));
            }
            var order = mapper.Map<Order>(input);
            order.Guid = Guid.NewGuid().ToString();
            order.DateTime = DateTime.Now;
            await orderRepository.AddAsync(order);

            var args = new PendingOrderEventArgs(order.StrategyId, order.Guid);
            var newEvent = new Event
            {
                EntityType = EntityType.TurtlesStrategy,
                EntityId = input.StrategyId,
                EventType = JsonSerializer.Serialize(EventType.NewExitPendingOrder),
                EventData = JsonSerializer.Serialize(args),
            };
            await eventRepository.AddAsync(newEvent);
            await unitOfWork.CompleteAsync();

            Stop();

            if (NewPendingOrder != null) NewPendingOrder(this, args);
        }

        public void Stop()
        {
            if (Status == StrategyStatus.Stopping && Status == StrategyStatus.Stopped) return;
            if (OnStatusChange != null)
            {
                OnStatusChange(this, new NewStatusEventArgs(StrategyStatus.Stopping));
                OnStatusChange(this, new NewStatusEventArgs(StrategyStatus.Stopped));
            }
        }

        private void ExitOrderCreator_OnStatusChange(object? sender, NewStatusEventArgs e)
        {
            LogInformation($"New status {e.Status}");
        }

        private void LogInformation(string message, params object[] args)
        {
            var logMessage = $"StrategyId {StrategyId}, step {nameof(ExitOrderCreator)}: ";
            if (args.Any()) logger.LogInformation(logMessage + message, args);
            else logger.LogInformation(logMessage + message, args);
        }
    }
}
