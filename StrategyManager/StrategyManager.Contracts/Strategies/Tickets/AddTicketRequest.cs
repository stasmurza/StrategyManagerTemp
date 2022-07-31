namespace StrategyManager.Contracts.Strategies.Tickets
{
    /// <summary>
    /// New ticket for collecting data
    /// </summary>
    public class AddTicketRequest : ContractBase
    {
        /// <summary>
        /// Code
        /// </summary>
        /// <example>BTCUSDT</example>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Name
        /// </summary>
        /// <example>BTC USDT</example>
        public string Name { get; set; } = String.Empty;
    }
}
