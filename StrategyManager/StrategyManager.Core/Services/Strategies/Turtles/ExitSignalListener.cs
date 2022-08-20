using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StrategyManager.Core.Models.DTOs.Strategies;
using StrategyManager.Core.Models.Options;
using StrategyManager.Core.Models.Services;
using StrategyManager.Core.Models.Services.MarketDataProvider;
using StrategyManager.Core.Models.Services.Strategies;
using StrategyManager.Core.Models.Services.Strategies.Turtles;
using StrategyManager.Core.Models.Store.Events;
using StrategyManager.Core.Repositories.Abstractions;
using StrategyManager.Core.Services.Abstractions;
using StrategyManager.Core.Services.Strategies.Abstractions;
using StrategyManager.Core.Services.Strategies.Turtles.Abstractions;
using System.Text.Json;

namespace StrategyManager.Core.Services.Strategies.Turtles
{
    public class ExitSignalListener : IExitSignalListener, IIdempotentStep, IDisposable
    {
        public StrategyStatus Status { get; private set; }
        public event EventHandler<NewStatusEventArgs>? OnStatusChange;
        public event EventHandler<ExitSignalEventArgs>? ExitSignal;

        private readonly IHistoryProvider historyProvider;
        private readonly TurtlesStrategyOptions options;
        private readonly IMarketDataProvider marketDataProvider;
        private readonly IRepository<Event> eventRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<EntrySignalListener> logger;
        private string instrumentCode = String.Empty;
        private string strategyId = String.Empty;
        private OrderDTO? entryOrder;
        private DateOnly currentDate;
        private decimal exitMaxPrice;
        private decimal exitMinPrice;
        private EventHandler<MarketDataEventArgs>? PriceChangedHandler;
        private bool disposed;

        public ExitSignalListener(
            IHistoryProvider historyProvider,
            IOptions<TurtlesStrategyOptions> options,
            IRepository<Event> eventRepository,
            IMarketDataProvider marketDataProvider,
            IUnitOfWork unitOfWork,
            ILogger<EntrySignalListener> logger)
        {
            this.historyProvider = historyProvider;
            this.options = options.Value;
            this.eventRepository = eventRepository;
            this.marketDataProvider = marketDataProvider;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            marketDataProvider.PriceChanged += PriceChangedHandler;
            OnStatusChange += ExitSignalListener_OnStatusChange;
        }

        public void Run(ExitSignalInput input)
        {
            if (input.EntryOrder is null) throw new ArgumentNullException(nameof(input.EntryOrder));

            if (OnStatusChange != null) OnStatusChange(this, new NewStatusEventArgs(StrategyStatus.Starting));

            instrumentCode = input.InstrumentCode;
            strategyId = input.StrategyId;
            entryOrder = input.EntryOrder;

            BuildState();

            marketDataProvider.Subscribe(instrumentCode, TimeFrame.Minute);

            if (PriceChangedHandler == null) PriceChangedHandler += MarketDataProvider_PriceChanged;
            if (OnStatusChange != null) OnStatusChange(this, new NewStatusEventArgs(StrategyStatus.Running));
        }

        private void MarketDataProvider_PriceChanged(object? sender, MarketDataEventArgs e)
        {
            Process(e.MarketData);
            LogInformation("Price changed", e);
        }

        public void Stop()
        {
            if (Status == StrategyStatus.Stopping && Status == StrategyStatus.Stopped) return;
            if (OnStatusChange != null) OnStatusChange(this, new NewStatusEventArgs(StrategyStatus.Stopping));
            PriceChangedHandler = null;
            marketDataProvider.Unsubscribe(instrumentCode, TimeFrame.Minute);
            if (OnStatusChange != null) OnStatusChange(this, new NewStatusEventArgs(StrategyStatus.Stopped));
        }

        private void BuildState()
        {
            currentDate = DateOnly.FromDateTime(DateTime.Now.Date);
            var startDate = currentDate.AddDays(-options.ExitPeriod).ToDateTime(TimeOnly.MinValue);
            var endDate = DateTime.Now;
            var history = historyProvider.GetHistory(instrumentCode, TimeFrame.Day, startDate, endDate);
            if (history is null) throw new ArgumentNullException(nameof(history));

            exitMaxPrice = history
                .MaxBy(i => i.MaxPrice)
                .MaxPrice;

            exitMinPrice = history
                .MinBy(i => i.MinPrice)
                .MinPrice;
        }

        private void Process(MarketData marketData)
        {
            if (currentDate != DateOnly.FromDateTime(marketData.DateAndTime)) BuildState();
            ExitLogic(marketData);
        }

        private void ExitLogic(MarketData marketData)
        {
            if (entryOrder is null) throw new ArgumentNullException(nameof(entryOrder));

            if (entryOrder.Direction == Direction.Short &&
                marketData.MaxPrice > exitMaxPrice) NewSignal(Direction.Long, exitMaxPrice);
            else if (entryOrder.Direction == Direction.Long &&
                marketData.MinPrice < exitMinPrice) NewSignal(Direction.Short, exitMinPrice);
        }

        private async void NewSignal(Direction direction, decimal price)
        {
            if (entryOrder is null) throw new ArgumentNullException(nameof(entryOrder));

            LogInformation($"New exit signal, direction {direction}, price {price}");

            var args = new ExitSignalEventArgs(direction, entryOrder.Volume, price);

            var newEvent = new Event
            {
                EntityType = EntityType.TurtlesStrategy,
                EntityId = strategyId,
                EventType = JsonSerializer.Serialize(EventType.NewExitSignal),
                EventData = JsonSerializer.Serialize(args),
            };

            await eventRepository.AddAsync(newEvent);
            await unitOfWork.CompleteAsync();
            Stop();
            if (ExitSignal != null) ExitSignal(this, args);
        }
        private void ExitSignalListener_OnStatusChange(object? sender, NewStatusEventArgs e)
        {
            LogInformation($"New status {e.Status}");
        }

        private void LogInformation(string message, params object[] args)
        {
            var logMessage = $"StrategyId {strategyId}, step {nameof(ExitSignalListener)}: ";
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
                Stop();
                marketDataProvider.PriceChanged -= PriceChangedHandler;
                OnStatusChange -= ExitSignalListener_OnStatusChange;
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            disposed = true;
        }

        ~ExitSignalListener() => Dispose(false);
    }
}
