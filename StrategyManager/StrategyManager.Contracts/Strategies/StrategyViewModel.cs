using StrategyManager.Contracts.Strategies.Tickets;

namespace StrategyManager.Contracts.Jobs
{
    /// <summary>
    /// Job view model
    /// </summary>
    public class StrategyViewModel
    {
        /// <summary>
        /// Id of job
        /// </summary>
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Name of job
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Should job be started with startup of service
        /// </summary>
        public bool StartWithService { get; set; } = false;

        /// <summary>
        /// Tickets for job
        /// </summary>
        public IEnumerable<TicketViewModel> Tickets { get; set; } = Enumerable.Empty<TicketViewModel>();
    }
}
