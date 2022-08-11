using Microsoft.Extensions.Options;
using StrategyManager.Core.Models.Options;
using StrategyManager.Core.Models.Services;
using StrategyManager.Core.Models.Services.Strategies;
using StrategyManager.Core.Services.Abstractions;
using StrategyManager.Core.Services.Abstractions.Strategies;

namespace StrategyManager.Core.Services.Strategies.Turtles
{
    public class TurtlesExitRules : IExitRules
    {
        /// <inheritdoc/>
        public event EventHandler<ClosePositionEventArgs>? NewSignal;
        private readonly IHistoryProvider historyProvider;
        private readonly TurtlesStrategyOptions options;
        private DateOnly CurrentDate { get; set; }
        private decimal ExitMaxPrice { get; set; }
        private decimal ExitMinPrice { get; set; }

        public TurtlesExitRules(
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

            ExitMaxPrice = history
                .MaxBy(i => i.MaxPrice)
                .MaxPrice;

            ExitMinPrice = history
                .MinBy(i => i.MinPrice)
                .MinPrice;
        }

        /// <inheritdoc/>
        public void Process(MarketData marketData, Position? activePosition)
        {
            if (CurrentDate != DateOnly.FromDateTime(marketData.DateAndTime)) BuildState();
            if (activePosition != null) ExitLogic(marketData, activePosition);
            if (ExitMaxPrice < marketData.MaxPrice) ExitMaxPrice = marketData.MaxPrice;
            if (ExitMinPrice > marketData.MinPrice) ExitMinPrice = marketData.MinPrice;
        }

        private void ExitLogic(MarketData marketData, Position activePosition)
        {
            if (activePosition is null) throw new ArgumentNullException("Position is not active");
            if (NewSignal == null) return;

            if ((activePosition.Direction == PositionDirection.Short && marketData.MaxPrice > ExitMaxPrice) ||
                (activePosition.Direction == PositionDirection.Long && marketData.MinPrice < ExitMinPrice))
            {
                //close position
                var args = new ClosePositionEventArgs(activePosition);
                NewSignal(this, args);
            }
        }
    }
}
