using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
    public class EntrySignalListener : IEntrySignalListener, IIdempotentStep, IDisposable
    {
        public StrategyStatus Status { get; private set; }
        public event EventHandler<NewStatusEventArgs>? OnStatusChange;
        public event EventHandler<EntrySignalEventArgs>? EntrySignal;

        private readonly IHistoryProvider historyProvider;
        private readonly TurtlesStrategyOptions options;
        private IMarketDataProvider marketDataProvider;
        private IRepository<Event> eventRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<EntrySignalListener> logger;
        private string InstrumentCode { get; set; } = String.Empty;
        private string StrategyId { get; set; } = String.Empty;
        private DateOnly CurrentDate { get; set; }
        private decimal EntryMaxPrice { get; set; }
        private decimal EntryMinPrice { get; set; }
        private EventHandler<MarketDataEventArgs>? PriceChangedHandler;
        private bool disposed;

        public EntrySignalListener(
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
            OnStatusChange += EntrySignalListener_OnStatusChange;
        }

        public void Run(EntrySignalInput input)
        {
            if (OnStatusChange != null) OnStatusChange(this, new NewStatusEventArgs(StrategyStatus.Starting));
            InstrumentCode = input.InstrumentCode;
            StrategyId = input.StrategyId;
            BuildState();
            marketDataProvider.Subscribe(InstrumentCode, TimeFrame.Minute);
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
            marketDataProvider.Unsubscribe(InstrumentCode, TimeFrame.Minute);
            if (OnStatusChange != null) OnStatusChange(this, new NewStatusEventArgs(StrategyStatus.Stopped));
        }

        private void BuildState()
        {
            CurrentDate = DateOnly.FromDateTime(DateTime.Now.Date);
            var startDate = CurrentDate.AddDays(-options.EntryPeriod).ToDateTime(TimeOnly.MinValue);
            var endDate = DateTime.Now.AddDays(-1);
            var history = historyProvider.GetHistory(InstrumentCode, TimeFrame.Day, startDate, endDate);
            if (history is null) throw new ArgumentNullException(nameof(history));

            EntryMaxPrice = history
                .MaxBy(i => i.MaxPrice)
                .MaxPrice;

            EntryMinPrice = history
                .MinBy(i => i.MinPrice)
                .MinPrice;
        }

        private void Process(MarketData marketData)
        {
            if (CurrentDate != DateOnly.FromDateTime(marketData.DateAndTime)) BuildState();
            EntryLogic(marketData);
        }

        private void EntryLogic(MarketData marketData)
        {
            if (marketData.MaxPrice > EntryMaxPrice) NewSignal(Direction.Long, EntryMaxPrice);
            else if (marketData.MinPrice < EntryMinPrice) NewSignal(Direction.Short, EntryMinPrice);
        }

        private async void NewSignal(Direction direction, decimal price)
        {
            LogInformation($"New entry signal, direction {direction}, price {price}");
            var args = new EntrySignalEventArgs(direction, price);
            var newEvent = new Event
            {
                EntityType = EntityType.TurtlesStrategy,
                EntityId = StrategyId,
                EventType = JsonSerializer.Serialize(EventType.NewEntrySignal),
                EventData = JsonSerializer.Serialize(args),
            };

            await eventRepository.AddAsync(newEvent);
            await unitOfWork.CompleteAsync();
            Stop();
            if (EntrySignal != null) EntrySignal(this, args);
        }

        private void EntrySignalListener_OnStatusChange(object? sender, NewStatusEventArgs e)
        {
            LogInformation($"New status {e.Status}");
        }

        private void LogInformation(string message, params object[] args)
        {
            var logMessage = $"StrategyId {StrategyId}, step {nameof(EntrySignalListener)}: ";
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
                OnStatusChange -= EntrySignalListener_OnStatusChange;
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            disposed = true;
        }

        ~EntrySignalListener() => Dispose(false);
    }
}
