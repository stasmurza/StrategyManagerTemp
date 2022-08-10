using Microsoft.Extensions.Options;
using StrategyManager.Core.Models.Options;
using StrategyManager.Core.Models.Services.Strategies;
using StrategyManager.Core.Services.Abstractions;
using StrategyManager.Core.Services.Abstractions.Strategies;

namespace StrategyManager.Core.Services.Strategies.Turtles
{
    public class TurtlesEntryRules : IEntryRules
    {
        /// <inheritdoc/>
        public event EventHandler<NewPositionEventArgs>? NewSignal;
        private readonly IHistoryProvider historyProvider;
        private readonly TurtlesStrategyOptions options;
        private DateOnly CurrentDate { get; set; }
        private decimal EntryMaxPrice { get; set; }
        private decimal EntryMinPrice { get; set; }

        public TurtlesEntryRules(
            IHistoryProvider historyProvider,
            IOptions<TurtlesStrategyOptions> options)
        {
            this.historyProvider = historyProvider;
            this.options = options.Value;
        }

        /// <inheritdoc/>
        public void BuildState()
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

        /// <inheritdoc/>
        public void Process(MarketData marketData, Position? activePosition)
        {
            if (CurrentDate != DateOnly.FromDateTime(marketData.DateAndTime)) BuildState();
            if (activePosition == null) EntryLogic(marketData);
            if (EntryMaxPrice < marketData.MaxPrice) EntryMaxPrice = marketData.MaxPrice;
            if (EntryMinPrice > marketData.MinPrice) EntryMinPrice = marketData.MinPrice;
        }

        private void EntryLogic(MarketData marketData)
        {
            if (marketData.MaxPrice > EntryMaxPrice)
            {
                //open long
                if (NewSignal != null)
                {
                    var args = new NewPositionEventArgs(PositionDirection.Long);
                    NewSignal(this, args);
                }
            }
            else if (marketData.MinPrice < EntryMinPrice)
            {
                //open short
                if (NewSignal != null)
                {
                    var args = new NewPositionEventArgs(PositionDirection.Short);
                    NewSignal(this, args);
                }
            }
        }
    }
}
