using StrategyManager.Contracts.Strategies.Tickets;

namespace StrategyManager.Contracts.Strategies
{
    /// <summary>
    /// Strategy view model
    /// </summary>
    public class StrategyViewModel
    {
        /// <summary>
        /// Id of strategy
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of strategy
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Should strategy be started with startup of service
        /// </summary>
        public bool StartWithService { get; set; } = false;

        /// <summary>
        /// Tickets for strategy
        /// </summary>
        public IEnumerable<TicketViewModel> Tickets { get; set; } = Enumerable.Empty<TicketViewModel>();
    }
}
