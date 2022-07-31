namespace StrategyManager.Contracts.Strategies.Tickets
{
    /// <summary>
    /// Ticket view model
    /// </summary>
    public class TicketViewModel
    {
        /// <summary>
        /// Id of ticket
        /// </summary>
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Code of ticket
        /// </summary>
        public string Code { get; set; } = String.Empty;

        /// <summary>
        /// Name of ticket
        /// </summary>
        public string Name { get; set; } = String.Empty;
    }
}
