using StrategyManager.Contracts.Strategies.Tickets;

namespace StrategyManager.Contracts.StrategiesReport
{
    /// <summary>
    /// Strategy view model
    /// </summary>
    public class StrategyViewModel
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
        public IEnumerable<TicketViewModel> Tickets { get; set; } = Enumerable.Empty<TicketViewModel>();
    }
}
