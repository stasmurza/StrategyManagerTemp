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
    public class ExitSignalListener : IExitSignalListener, IIdempotentCommand
    {
        public event EventHandler<EventArgs>? Started;
        public event EventHandler<EventArgs>? Stopped;
        public event EventHandler<ExitSignalEventArgs>? ExitSignal;

        private readonly IHistoryProvider historyProvider;
        private readonly TurtlesStrategyOptions options;
        private IMarketDataProvider marketDataProvider;
        private IRepository<Event> eventRepository;
        private readonly IUnitOfWork unitOfWork;
        private string InstrumentCode { get; set; } = String.Empty;
        private string StrategyId { get; set; } = String.Empty;
        private PositionDirection PositionDirection { get; set; }
        private DateOnly CurrentDate { get; set; }
        private decimal ExitMaxPrice { get; set; }
        private decimal ExitMinPrice { get; set; }
        private EventHandler<MarketDataEventArgs>? PriceChangedHandler;

        public ExitSignalListener(
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

        public void Run(ExitSignalInput input)
        {
            if (Started != null) Started(this, EventArgs.Empty);
            InstrumentCode = input.InstrumentCode;
            StrategyId = input.StrategyId;
            PositionDirection = input.Direction;
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

        private void BuildState()
        {
            CurrentDate = DateOnly.FromDateTime(DateTime.Now.Date);
            var startDate = CurrentDate.AddDays(-options.ExitPeriod).ToDateTime(TimeOnly.MinValue);
            var endDate = DateTime.Now;
            var history = historyProvider.GetHistory(InstrumentCode, TimeFrame.Day, startDate, endDate);
            if (history is null) throw new ArgumentNullException(nameof(history));

            ExitMaxPrice = history
                .MaxBy(i => i.MaxPrice)
                .MaxPrice;

            ExitMinPrice = history
                .MinBy(i => i.MinPrice)
                .MinPrice;
        }

        private void Process(MarketData marketData)
        {
            if (CurrentDate != DateOnly.FromDateTime(marketData.DateAndTime)) BuildState();
            ExitLogic(marketData);
        }

        private void ExitLogic(MarketData marketData)
        {
            if (PositionDirection == PositionDirection.Short &&
                marketData.MaxPrice > ExitMaxPrice) NewSignal(PositionDirection.Long);
            else if (PositionDirection == PositionDirection.Long &&
                marketData.MinPrice < ExitMinPrice) NewSignal(PositionDirection.Short);
        }

        private void NewSignal(PositionDirection direction)
        {
            var args = new ExitSignalEventArgs
            {
                Direction = direction
            };

            var newEvent = new Event
            {
                EntityType = EntityType.TurtlesStrategy,
                EntityId = StrategyId,
                EventType = JsonSerializer.Serialize(EventType.NewExitSignal),
                EventData = JsonSerializer.Serialize(args),
            };

            var task = eventRepository.AddAsync(newEvent);
            task.Wait();
            task = unitOfWork.CompleteAsync();
            task.Wait();
            Stop();
            if (ExitSignal != null) ExitSignal(this, args);
        }
    }
}
