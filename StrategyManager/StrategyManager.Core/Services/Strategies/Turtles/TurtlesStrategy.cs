using StrategyManager.Core.Models.Options;
using StrategyManager.Core.Models.Services.Strategies;
using StrategyManager.Core.Models.Services.Strategies.Turtles;
using StrategyManager.Core.Models.Store.Events;
using StrategyManager.Core.Repositories.Abstractions;
using StrategyManager.Core.Services.Abstractions.Strategies;
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

        private String StrategyId { get; set; } = string.Empty;

        private readonly InstrumentOptions instrumentOptions;
        private IRepository<Event> eventRepository;
        private IEntrySignalListener EntrySignalListener { get; set; }
        private IPendingOrderCreator EntryOrderCreator { get; set; }
        private IOrderHandler EntryOrderHandler { get; set; }
        private IExitSignalListener ExitSignalListener { get; set; }
        private IPendingOrderCreator ExitOrderCreator { get; set; }
        private IOrderHandler ExitOrderHandler { get; set; }

        private bool disposed;

        public TurtlesStrategy(
            IRepository<Event> eventRepository,
            IEntrySignalListener entrySignalListener,
            IPendingOrderCreator entryOrderCreator,
            IOrderHandler entryOrderHandler,
            IExitSignalListener exitSignalListener,
            IPendingOrderCreator exitOrderCreator,
            IOrderHandler exitOrderHandler)
        {
            this.eventRepository = eventRepository;
            EntrySignalListener = entrySignalListener;
            EntryOrderCreator = entryOrderCreator;
            EntryOrderHandler = entryOrderHandler;
            ExitSignalListener = exitSignalListener;
            ExitOrderCreator = exitOrderCreator;
            ExitOrderHandler = exitOrderHandler;

            EntrySignalListener.EntrySignal += EntrySignalListener_EntrySignal;
            EntryOrderCreator.NewPendingOrder += EntryOrderCreator_NewPendingOrder;
            EntryOrderHandler.OrderCancelled += EntryOrderHandler_OrderCancelled;
            EntryOrderHandler.OrderRejected += EntryOrderHandler_OrderRejected;
            EntryOrderHandler.OrderFilled += EntryOrderHandler_OrderFilled;
            ExitSignalListener.ExitSignal += ExitSignalListener_ExitSignal;
            ExitOrderCreator.NewPendingOrder += ExitOrderCreator_NewPendingOrder;
            ExitOrderHandler.OrderCancelled += ExitOrderHandler_OrderCancelled;
            ExitOrderHandler.OrderRejected += ExitOrderHandler_OrderRejected;
            ExitOrderHandler.OrderFilled += ExitOrderHandler_OrderFilled;
        }

        public async Task StartAsync(string instrumentCode, CancellationTokenSource cancellationTokenSource)
        {
            InstrumentCode = instrumentCode;
            StrategyId = StrategyCode.ToString() + instrumentCode;
            await RunAsync();
        }

        public async Task StopAsync()
        {
            Status = StrategyStatus.Stopping;
            IIdempotentCommand result = StrategyStep switch
            {
                StrategyStep.ListeningEntrySignal => EntrySignalListener,
                StrategyStep.CreatingEntryPendingOrder => EntryOrderCreator,
                StrategyStep.HandlingEntryOrder => EntryOrderHandler,
                StrategyStep.ListeningExitSignal => ExitSignalListener,
                StrategyStep.CreatingExitPendingOrder => ExitOrderCreator,
                StrategyStep.HandlingExitOrder => ExitOrderHandler,
                _ => throw new ArgumentOutOfRangeException(nameof(StrategyStep), $"Not expected state value: {StrategyStep}"),
            };
            result.Stop();
            Status = StrategyStatus.Stopped;
        }

        private async Task RunAsync()
        {
            var lastEvent = await eventRepository.FirstOrDefaultAsync(i => i.EntityId == StrategyId, "Desc");
            if (lastEvent is null)
            {
                RunEntryListener();
                return;
            }

            var eventType = JsonSerializer.Deserialize<Models.Services.Strategies.Turtles.EventType>(lastEvent.EventData);
            switch (eventType)
            {
                case Models.Services.Strategies.Turtles.EventType.NewEntrySignal:
                    var args = JsonSerializer.Deserialize<EntrySignalEventArgs>(lastEvent.EventData);
                    if (args == null) throw new ArgumentNullException($"{nameof(args)} is null");
                    EntrySignalListener_EntrySignal(EntrySignalListener, args);
                    break;
                case Models.Services.Strategies.Turtles.EventType.NewEntryPendingOrder:
                    args = JsonSerializer.Deserialize<EntrySignalEventArgs>(lastEvent.EventData);
                    if (args == null) throw new ArgumentNullException($"{nameof(args)} is null");
                    EntrySignalListener_EntrySignal(EntrySignalListener, args);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventType), $"Not expected state value: {eventType}");
            }
        }

        private void RunEntryListener()
        {
            var input = new EntrySignalInput
            {
                InstrumentCode = InstrumentCode,
            };
            EntrySignalListener.Run(input);
        }

        private void EntrySignalListener_EntrySignal(object? sender, EntrySignalEventArgs e)
        {
            if (Status == StrategyStatus.Stopping) return;
            if (Status != StrategyStatus.Running)
            {
                var message = $"Event an not be processed. Strategy {StrategyId} is in status {Status}";
                throw new InvalidOperationException(message);
            }

            StrategyStep = StrategyStep.CreatingEntryPendingOrder;
            var input = new PendingOrderInput
            {
                StrategyId = StrategyId,
                InstrumentCode = InstrumentCode,
                Direction = e.Direction,
                Volume = instrumentOptions.Volume,
            };
            var task = EntryOrderCreator.CreatePendingOrderAsync(input);
            task.Wait();
        }

        private void EntryOrderCreator_NewPendingOrder(object? sender, PendingOrderEventArgs e)
        {
            if (Status == StrategyStatus.Stopping) return;
            if (Status != StrategyStatus.Running)
            {
                var message = $"Event an not be processed. Strategy {StrategyId} is in status {Status}";
                throw new InvalidOperationException(message);
            }

            StrategyStep = StrategyStep.HandlingEntryOrder;
            var input = new OrderHandlerInput
            {
                OrderId = e.OrderId,
                InstrumentCode = InstrumentCode,
                Direction = e.Direction,
                Volume = instrumentOptions.Volume,
            };
            EntryOrderHandler.HandleOrder(input);
        }

        private void EntryOrderHandler_OrderFilled(object? sender, OrderHandlerEventArgs e)
        {
            if (Status == StrategyStatus.Stopping) return;
            if (Status != StrategyStatus.Running)
            {
                var message = $"Event can not be processed. Strategy {StrategyId} is in status {Status}";
                throw new InvalidOperationException(message);
            }

            StrategyStep = StrategyStep.ListeningExitSignal;
            var input = new ExitSignalInput
            {
                InstrumentCode = InstrumentCode,
                Direction = e.Direction,
            };
            ExitSignalListener.Run(input);
        }

        private void EntryOrderHandler_OrderRejected(object? sender, OrderHandlerEventArgs e)
        {
            var message = $"Unsuccessful attemp to register order {e.OrderId}. Order rejected.";
            throw new InvalidOperationException(message);
        }

        private void EntryOrderHandler_OrderCancelled(object? sender, OrderHandlerEventArgs e)
        {
            var message = $"Unsuccessful attemp to register order {e.OrderId}. Order cancelled.";
            throw new InvalidOperationException(message);
        }

        private void ExitSignalListener_ExitSignal(object? sender, ExitSignalEventArgs e)
        {
            if (Status == StrategyStatus.Stopping) return;
            if (Status != StrategyStatus.Running)
            {
                var message = $"Event an not be processed. Strategy {StrategyId} is in status {Status}";
                throw new InvalidOperationException(message);
            }

            StrategyStep = StrategyStep.CreatingExitPendingOrder;
            var input = new PendingOrderInput
            {
                InstrumentCode = InstrumentCode,
                Direction = e.Direction,
                Volume = instrumentOptions.Volume,
            };
            var task = ExitOrderCreator.CreatePendingOrderAsync(input);
            task.Wait();
        }

        private void ExitOrderCreator_NewPendingOrder(object? sender, PendingOrderEventArgs e)
        {
            if (Status == StrategyStatus.Stopping) return;
            if (Status != StrategyStatus.Running)
            {
                var message = $"Event an not be processed. Strategy {StrategyId} is in status {Status}";
                throw new InvalidOperationException(message);
            }

            StrategyStep = StrategyStep.HandlingExitOrder;
            var input = new OrderHandlerInput
            {
                OrderId = e.OrderId,
                InstrumentCode = InstrumentCode,
                Direction = e.Direction,
                Volume = instrumentOptions.Volume,
            };
            ExitOrderHandler.HandleOrder(input);
        }

        private void ExitOrderHandler_OrderFilled(object? sender, OrderHandlerEventArgs e)
        {
            if (Status == StrategyStatus.Stopping) return;
            if (Status != StrategyStatus.Running)
            {
                var message = $"Event an not be processed. Strategy {StrategyId} is in status {Status}";
                throw new InvalidOperationException(message);
            }

            StrategyStep = StrategyStep.ListeningEntrySignal;
            var input = new EntrySignalInput
            {
                InstrumentCode = InstrumentCode,
            };
            EntrySignalListener.Run(input);
        }

        private void ExitOrderHandler_OrderRejected(object? sender, OrderHandlerEventArgs e)
        {
            var message = $"Unsuccessful attemp to register order {e.OrderId}. Order rejected.";
            throw new InvalidOperationException(message);
        }

        private void ExitOrderHandler_OrderCancelled(object? sender, OrderHandlerEventArgs e)
        {
            var message = $"Unsuccessful attemp to register order {e.OrderId}. Order cancelled.";
            throw new InvalidOperationException(message);
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
                EntrySignalListener.EntrySignal -= EntrySignalListener_EntrySignal;
                EntryOrderCreator.NewPendingOrder -= EntryOrderCreator_NewPendingOrder;
                EntryOrderHandler.OrderCancelled -= EntryOrderHandler_OrderCancelled;
                EntryOrderHandler.OrderRejected -= EntryOrderHandler_OrderRejected;
                EntryOrderHandler.OrderFilled -= EntryOrderHandler_OrderFilled;
                ExitSignalListener.ExitSignal -= ExitSignalListener_ExitSignal;
                ExitOrderCreator.NewPendingOrder -= ExitOrderCreator_NewPendingOrder;
                ExitOrderHandler.OrderCancelled -= ExitOrderHandler_OrderCancelled;
                ExitOrderHandler.OrderRejected -= ExitOrderHandler_OrderRejected;
                ExitOrderHandler.OrderFilled -= ExitOrderHandler_OrderFilled;
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            disposed = true;
        }

        ~TurtlesStrategy() => Dispose(false);
    }
}
