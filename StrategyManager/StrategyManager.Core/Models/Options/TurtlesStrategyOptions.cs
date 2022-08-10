namespace StrategyManager.Core.Models.Options
{
    public class TurtlesStrategyOptions
    {
        /// <summary>
        /// Entry period, days
        /// </summary>
        public int EntryPeriod { get; set; } = default;

        /// <summary>
        /// Exit period, days
        /// </summary>
        public int ExitPeriod { get; set; } = default;

        /// <summary>
        /// Stop loss limit, percents
        /// </summary>
        public int StopLossLimit { get; set; } = default;
    }
}
