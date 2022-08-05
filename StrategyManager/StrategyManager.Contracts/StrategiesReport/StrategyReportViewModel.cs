namespace StrategyManager.Contracts.StrategiesReport
{
    /// <summary>
    /// Strategy view model
    /// </summary>
    public class StrategyReportViewModel
    {
        /// <summary>
        /// Id of strategy
        /// </summary>
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Name of strategy
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Tickets for strategy
        /// </summary>
        public IEnumerable<TicketReportViewModel> Tickets { get; set; } = Enumerable.Empty<TicketReportViewModel>();
    }
}
