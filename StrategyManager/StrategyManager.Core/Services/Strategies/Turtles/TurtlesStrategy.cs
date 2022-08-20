using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StrategyManager.Core.Models.Options;
using StrategyManager.Core.Models.Services.Strategies;
using StrategyManager.Core.Models.Services.Strategies.Turtles;
using StrategyManager.Core.Models.Store.Events;
using StrategyManager.Core.Repositories.Abstractions;
using StrategyManager.Core.Services.Strategies.Abstractions;
using StrategyManager.Core.Services.Strategies.Turtles.Abstractions;
using System.Text.Json;

namespace StrategyManager.Core.Services.Strategies.Turtles
{
    public class TurtlesStrategy : ITurtlesStrategy
    {
        public StrategyStatus Status { get; private set; } = StrategyStatus.Stopped;
        public StrategyCode StrategyCode { get; private set; } = StrategyCode.Turtles;
        public string InstrumentCode { get; private set; } = string.Empty;
        public DateTime LastActive { get; private set; } = default;
        public StrategyStep StrategyStep { get; private set; }

        public event EventHandler<NewStatusEventArgs>? OnStatusChange;

        private String StrategyId { get; set; } = string.Empty;
        private readonly InstrumentOptions instrumentOptions;
        private IRepository<Event> eventRepository;
        private IEntrySignalListener entrySignalListener;
        private IPendingOrderCreator entryOrderCreator;
        private IOrderHandler entryOrderHandler;
        private IExitSignalListener exitSignalListener;
        private IPendingOrderCreator exitOrderCreator;
        private IOrderHandler exitOrderHandler;
        private readonly IMapper mapper;
        private readonly ILogger<TurtlesStrategy> logger;
        private bool disposed;

        public TurtlesStrategy(
            IRepository<Event> eventRepository,
            IEntrySignalListener entrySignalListener,
            IPendingOrderCreator entryOrderCreator,
            IOrderHandler entryOrderHandler,
            IExitSignalListener exitSignalListener,
            IPendingOrderCreator exitOrderCreator,
            IOrderHandler exitOrderHandler,
            IOptions<InstrumentOptions> instrumentOptions,
            IMapper mapper,
            ILogger<TurtlesStrategy> logger)
        {
            this.eventRepository = eventRepository;
            this.entrySignalListener = entrySignalListener;
            this.entryOrderCreator = entryOrderCreator;
            this.entryOrderHandler = entryOrderHandler;
            this.exitSignalListener = exitSignalListener;
            this.exitOrderCreator = exitOrderCreator;
            this.exitOrderHandler = exitOrderHandler;
            this.instrumentOptions = instrumentOptions.Value;
            this.mapper = mapper;
            this.logger = logger;

            this.entrySignalListener.EntrySignal += EntrySignalListener_EntrySignal;
            this.entryOrderCreator.NewPendingOrder += EntryOrderCreator_NewPendingOrder;
            this.entryOrderHandler.OrderCancelled += EntryOrderHandler_OrderCancelled;
            this.entryOrderHandler.OrderRejected += EntryOrderHandler_OrderRejected;
            this.entryOrderHandler.OrderFilled += EntryOrderHandler_OrderFilled;
            this.exitSignalListener.ExitSignal += ExitSignalListener_ExitSignal;
            this.exitOrderCreator.NewPendingOrder += ExitOrderCreator_NewPendingOrder;
            this.exitOrderHandler.OrderCancelled += ExitOrderHandler_OrderCancelled;
            this.exitOrderHandler.OrderRejected += ExitOrderHandler_OrderRejected;
            this.exitOrderHandler.OrderFilled += ExitOrderHandler_OrderFilled;
            OnStatusChange += TurtlesStrategy_OnStatusChange; ;
        }

        public async Task StartAsync(string instrumentCode, CancellationTokenSource cancellationTokenSource)
        {
            if (OnStatusChange != null) OnStatusChange(this, new NewStatusEventArgs(StrategyStatus.Starting));
            InstrumentCode = instrumentCode;
            StrategyId = String.Format(IStrategy.StartegyIdPattern, StrategyCode, instrumentCode);
            await RunAsync();
            if (OnStatusChange != null) OnStatusChange(this, new NewStatusEventArgs(StrategyStatus.Running));
        }

        public async Task StopAsync()
        {
            if (Status == StrategyStatus.Stopping && Status == StrategyStatus.Stopped) return;
            if (OnStatusChange != null) OnStatusChange(this, new NewStatusEventArgs(StrategyStatus.Stopping));

            Status = StrategyStatus.Stopping;
            IIdempotentStep result = GetCurrentCommand();
            result.Stop();
            Status = StrategyStatus.Stopped;

            if (OnStatusChange != null) OnStatusChange(this, new NewStatusEventArgs(StrategyStatus.Stopped));
        }

        private IIdempotentStep GetCurrentCommand()
        {
            return StrategyStep switch
            {
                StrategyStep.ListeningEntrySignal => entrySignalListener,
                StrategyStep.CreatingEntryPendingOrder => entryOrderCreator,
                StrategyStep.HandlingEntryOrder => entryOrderHandler,
                StrategyStep.ListeningExitSignal => exitSignalListener,
                StrategyStep.CreatingExitPendingOrder => exitOrderCreator,
                StrategyStep.HandlingExitOrder => exitOrderHandler,
                _ => throw new ArgumentOutOfRangeException(nameof(StrategyStep), $"Not expected state value: {StrategyStep}"),
            };
        }

        private async Task RunAsync()
        {
            var lastEvent = await eventRepository.FirstOrDefaultAsync(
                i => i.Id,
                true,
                i => i.EntityId == StrategyId);

            if (lastEvent is null)
            {
                RunEntryListener();
                return;
            }

            var eventType = JsonSerializer.Deserialize<EventType>(lastEvent.EventType);
            switch (eventType)
            {
                case EventType.NewEntrySignal:
                    {
                        var args = JsonSerializer.Deserialize<EntrySignalEventArgs>(lastEvent.EventData);
                        if (args == null) throw new ArgumentNullException($"{nameof(args)} is null");
                        EntrySignalListener_EntrySignal(entrySignalListener, args);
                        break;
                    }
                case EventType.NewEntryPendingOrder:
                    {
                        var args = JsonSerializer.Deserialize<PendingOrderEventArgs>(lastEvent.EventData);
                        if (args == null) throw new ArgumentNullException($"{nameof(args)} is null");
                        EntryOrderCreator_NewPendingOrder(entrySignalListener, args);
                        break;
                    }
                case EventType.EntryOrderFilled:
                    {
                        var args = JsonSerializer.Deserialize<OrderHandlerEventArgs>(lastEvent.EventData);
                        if (args == null) throw new ArgumentNullException($"{nameof(args)} is null");
                        EntryOrderHandler_OrderFilled(entrySignalListener, args);
                        break;
                    }
                case EventType.NewExitSignal:
                    {
                        var args = JsonSerializer.Deserialize<ExitSignalEventArgs>(lastEvent.EventData);
                        if (args == null) throw new ArgumentNullException($"{nameof(args)} is null");
                        ExitSignalListener_ExitSignal(entrySignalListener, args);
                        break;
                    }
                case EventType.NewExitPendingOrder:
                    {
                        var args = JsonSerializer.Deserialize<PendingOrderEventArgs>(lastEvent.EventData);
                        if (args == null) throw new ArgumentNullException($"{nameof(args)} is null");
                        ExitOrderCreator_NewPendingOrder(entrySignalListener, args);
                        break;
                    }
                case EventType.ExitOrderFilled:
                    {
                        var args = JsonSerializer.Deserialize<OrderHandlerEventArgs>(lastEvent.EventData);
                        if (args == null) throw new ArgumentNullException($"{nameof(args)} is null");
                        ExitOrderHandler_OrderFilled(entrySignalListener, args);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventType), $"Not expected state value: {eventType}");
            }
        }

        private void RunEntryListener()
        {
            var input = new EntrySignalInput
            {
                StrategyId = StrategyId,
                InstrumentCode = InstrumentCode,
            };
            entrySignalListener.Run(input);
        }

        private async void EntrySignalListener_EntrySignal(object? sender, EntrySignalEventArgs e)
        {
            OnEventHandler();
            StrategyStep = StrategyStep.CreatingEntryPendingOrder;
            var input = new PendingOrderInput(
                StrategyId,
                InstrumentCode,
                e.Direction,
                instrumentOptions.Volume,
                e.Price);
            await entryOrderCreator.CreatePendingOrderAsync(input);
        }

        private void EntryOrderCreator_NewPendingOrder(object? sender, PendingOrderEventArgs e)
        {
            OnEventHandler();
            StrategyStep = StrategyStep.HandlingEntryOrder;
            var input = new OrderHandlerInput(e.StrategyId, e.Order);
            entryOrderHandler.HandleOrder(input);
        }

        private void EntryOrderHandler_OrderFilled(object? sender, OrderHandlerEventArgs e)
        {
            OnEventHandler();
            StrategyStep = StrategyStep.ListeningExitSignal;
            var input = new ExitSignalInput(StrategyId, InstrumentCode, e.Order);
            exitSignalListener.Run(input);
        }

        private void EntryOrderHandler_OrderRejected(object? sender, OrderHandlerEventArgs e)
        {
            OnEventHandler();
            var message = $"Unsuccessful attempt to register order {e.Order.Id}. Order rejected.";

            throw new InvalidOperationException(message);
        }

        private void EntryOrderHandler_OrderCancelled(object? sender, OrderHandlerEventArgs e)
        {
            OnEventHandler();
            var message = $"Unsuccessful attempt to register order {e.Order.Id}. Order cancelled.";
            throw new InvalidOperationException(message);
        }

        private void ExitSignalListener_ExitSignal(object? sender, ExitSignalEventArgs e)
        {
            OnEventHandler();
            StrategyStep = StrategyStep.CreatingExitPendingOrder;
            var input = new PendingOrderInput(
                StrategyId,
                InstrumentCode,
                e.Direction,
                e.Volume,
                e.Price);
            var task = exitOrderCreator.CreatePendingOrderAsync(input);
            task.Wait();
        }

        private void ExitOrderCreator_NewPendingOrder(object? sender, PendingOrderEventArgs e)
        {
            OnEventHandler();
            StrategyStep = StrategyStep.HandlingExitOrder;
            var input = new OrderHandlerInput(StrategyId, e.Order);
            exitOrderHandler.HandleOrder(input);
        }

        private void ExitOrderHandler_OrderFilled(object? sender, OrderHandlerEventArgs e)
        {
            OnEventHandler();
            StrategyStep = StrategyStep.ListeningEntrySignal;
            var input = new EntrySignalInput
            {
                InstrumentCode = InstrumentCode,
            };
            entrySignalListener.Run(input);
        }

        private void ExitOrderHandler_OrderRejected(object? sender, OrderHandlerEventArgs e)
        {
            OnEventHandler();
            var message = $"Unsuccessful attemp to register order {e.Order.Id}. Order rejected.";
            throw new InvalidOperationException(message);
        }

        private void ExitOrderHandler_OrderCancelled(object? sender, OrderHandlerEventArgs e)
        {
            OnEventHandler();
            var message = $"Unsuccessful attemp to register order {e.Order.Id}. Order cancelled.";
            throw new InvalidOperationException(message);
        }

        private void OnEventHandler()
        {
            if (Status == StrategyStatus.Stopping) return;
            if (Status != StrategyStatus.Running)
            {
                var message = $"Event can not be processed. Strategy {StrategyId} is in status {Status}";
                throw new InvalidOperationException(message);
            }
        }

        private void TurtlesStrategy_OnStatusChange(object? sender, NewStatusEventArgs e)
        {
            LogInformation($"New status {e.Status}");
        }

        private void LogInformation(string message, params object[] args)
        {
            var logMessage = $"StrategyId {StrategyId}: ";
            if (args.Any()) logger.LogInformation(logMessage + message, args);
            else logger.LogInformation(logMessage + message, args);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                // TODO: dispose managed state (managed objects (resources)).
                entrySignalListener.EntrySignal -= EntrySignalListener_EntrySignal;
                entryOrderCreator.NewPendingOrder -= EntryOrderCreator_NewPendingOrder;
                entryOrderHandler.OrderCancelled -= EntryOrderHandler_OrderCancelled;
                entryOrderHandler.OrderRejected -= EntryOrderHandler_OrderRejected;
                entryOrderHandler.OrderFilled -= EntryOrderHandler_OrderFilled;
                exitSignalListener.ExitSignal -= ExitSignalListener_ExitSignal;
                exitOrderCreator.NewPendingOrder -= ExitOrderCreator_NewPendingOrder;
                exitOrderHandler.OrderCancelled -= ExitOrderHandler_OrderCancelled;
                exitOrderHandler.OrderRejected -= ExitOrderHandler_OrderRejected;
                exitOrderHandler.OrderFilled -= ExitOrderHandler_OrderFilled;

                OnStatusChange -= TurtlesStrategy_OnStatusChange;
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            disposed = true;
        }

        ~TurtlesStrategy() => Dispose(false);
    }
}
