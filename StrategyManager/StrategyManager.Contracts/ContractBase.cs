namespace StrategyManager.Contracts
{
    /// <summary>
    /// Base class for contracts
    /// </summary>
    public class ContractBase
    {
        /// <summary>
        /// Correlation id
        /// </summary>
        public string CorrelationId { get; set; } = String.Empty;
    }
}
