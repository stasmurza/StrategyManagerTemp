using Microsoft.Extensions.Options;
using StrategyManager.Core.Models.Options;
using StrategyManager.Core.Models.Services;
using StrategyManager.Core.Models.Services.MarketDataProvider;
using StrategyManager.Core.Models.Services.Strategies;
using StrategyManager.Core.Models.Services.Strategies.Turtles;
using StrategyManager.Core.Models.Store.Events;
using StrategyManager.Core.Repositories.Abstractions;
using StrategyManager.Core.Services.Abstractions;
using StrategyManager.Core.Services.Abstractions.Strategies;
using System.Text.Json;

namespace StrategyManager.Core.Services.Strategies.Turtles
{
    public class EntrySignalListener : IEntrySignalListener, IIdempotentCommand
    {
        public event EventHandler<EventArgs>? Start;
        public event EventHandler<EventArgs>? Finish;
        public event EventHandler<EventArgs>? Error;
        public event EventHandler<EntrySignalEventArgs>? EntrySignal;

        private readonly IHistoryProvider historyProvider;
        private readonly TurtlesStrategyOptions options;
        private IRepository<Event> eventRepository;
        private string InstrumentCode { get; set; }
        private DateOnly CurrentDate { get; set; }
        private decimal EntryMaxPrice { get; set; }
        private decimal EntryMinPrice { get; set; }
        private EventHandler<MarketDataEventArgs>? MarketDataHandler;

        public EntrySignalListener(
            IHistoryProvider historyProvider,
            IOptions<TurtlesStrategyOptions> options)
        {
            this.historyProvider = historyProvider;
            this.options = options.Value;
        }

        public void Run(EntrySignalInput input)
        {
            InstrumentCode = input.InstrumentCode;
            BuildState();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        private void BuildState()
        {
            CurrentDate = DateOnly.FromDateTime(DateTime.Now.Date);
            var startDate = CurrentDate.AddDays(-options.EntryPeriod).ToDateTime(TimeOnly.MinValue);
            var endDate = DateTime.Now;
            var history = historyProvider.GetHistory(TimeFrame.Day, startDate, endDate);
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
            string jsonString = JsonSerializer.Serialize(args);
            var newEvent = new Event
            {
                EventType = "NewSignal",
                EntityType = "TurtlesStrategy",
                EntityId = "StrategyId",
                EventData = jsonString,
            };

            eventRepository.CreateAsync(newEvent);

            if (EntrySignal != null) EntrySignal(this, args);
        }
    }
}
