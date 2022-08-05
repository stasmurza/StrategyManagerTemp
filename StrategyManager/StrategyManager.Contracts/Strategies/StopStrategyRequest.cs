namespace StrategyManager.Contracts.Strategies
{
    /// <summary>
    /// Stop strategy by id
    /// </summary>
    public class StopStrategyRequest
    {
        /// <summary>
        /// Id of strategy
        /// </summary>
        public string Id { get; set; } = String.Empty;
    }
}
