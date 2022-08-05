namespace StrategyManager.Contracts.StrategiesReport
{
    /// <summary>
    /// Ticket view model
    /// </summary>
    public class TicketReportViewModel
    {
        /// <summary>
        /// Code of ticket
        /// </summary>
        public string Code { get; set; } = String.Empty;

        /// <summary>
        /// Status of ticket
        /// </summary>
        public string Status { get; set; } = String.Empty;
    }
}
