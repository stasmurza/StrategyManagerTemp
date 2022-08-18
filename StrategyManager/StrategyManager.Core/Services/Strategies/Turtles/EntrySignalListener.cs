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
    public class EntrySignalListener : IEntrySignalListener, IIdempotentCommand
    {
        public event EventHandler<EventArgs>? Started;
        public event EventHandler<EventArgs>? Stopped;
        public event EventHandler<EntrySignalEventArgs>? EntrySignal;

        private readonly IHistoryProvider historyProvider;
        private readonly TurtlesStrategyOptions options;
        private IMarketDataProvider marketDataProvider;
        private IRepository<Event> eventRepository;
        private readonly IUnitOfWork unitOfWork;
        private string InstrumentCode { get; set; } = String.Empty;
        private string StrategyId { get; set; } = String.Empty;
        private DateOnly CurrentDate { get; set; }
        private decimal EntryMaxPrice { get; set; }
        private decimal EntryMinPrice { get; set; }
        private EventHandler<MarketDataEventArgs>? PriceChangedHandler;

        public EntrySignalListener(
            IHistoryProvider historyProvider,
            IOptions<TurtlesStrategyOptions> options,
            IRepository<Event> eventRepository,
            IMarketDataProvider marketDataProvider,
            IUnitOfWork unitOfWork)
        {
            this.historyProvider = historyProvider;
            this.options = options.Value;
            this.eventRepository = eventRepository;
            this.marketDataProvider = marketDataProvider;
            this.unitOfWork = unitOfWork;
            marketDataProvider.PriceChanged += PriceChangedHandler;
        }

        public void Run(EntrySignalInput input)
        {
            InstrumentCode = input.InstrumentCode;
            StrategyId = input.StrategyId;
            BuildState();
            marketDataProvider.Subscribe(InstrumentCode, TimeFrame.Minute);
            if (PriceChangedHandler == null) PriceChangedHandler += MarketDataProvider_PriceChanged;
            if (Started != null) Started(this, EventArgs.Empty);
        }

        private void MarketDataProvider_PriceChanged(object? sender, MarketDataEventArgs e)
        {
            Process(e.MarketData);
        }

        public void Stop()
        {
            PriceChangedHandler = null;
            marketDataProvider.Unsubscribe(InstrumentCode, TimeFrame.Minute);
            if (Stopped != null) Stopped(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        private void BuildState()
        {
            CurrentDate = DateOnly.FromDateTime(DateTime.Now.Date);
            var startDate = CurrentDate.AddDays(-options.EntryPeriod).ToDateTime(TimeOnly.MinValue);
            var endDate = DateTime.Now;
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
            if (marketData.MaxPrice > EntryMaxPrice) NewSignal(PositionDirection.Long);
            else if (marketData.MinPrice < EntryMinPrice) NewSignal(PositionDirection.Short);
        }

        private void NewSignal(PositionDirection direction)
        {
            var args = new EntrySignalEventArgs
            {
                Direction = direction
            };
            var newEvent = new Event
            {
                EntityType = EntityType.TurtlesStrategy,
                EntityId = StrategyId,
                EventType = JsonSerializer.Serialize(EventType.NewEntrySignal),
                EventData = JsonSerializer.Serialize(args),
            };

            var task = eventRepository.AddAsync(newEvent);
            task.Wait();
            task = unitOfWork.CompleteAsync();
            task.Wait();
            Stop();
            if (EntrySignal != null) EntrySignal(this, args);
        }
    }
}
