namespace StrategyManager.Contracts.Strategies.Tickets
{
    /// <summary>
    /// Remove ticket from collecting data
    /// </summary>
    public class RemoveTicketRequest : ContractBase
    {
        /// <summary>
        /// Code
        /// </summary>
        /// <example>BTCUSDT</example>
        public string Code { get; set; } = string.Empty;
    }
}
