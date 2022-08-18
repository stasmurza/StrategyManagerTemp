namespace StrategyManager.Contracts.Strategies
{
    /// <summary>
    /// Start strategy by id
    /// </summary>
    public class RunStrategyRequest
    {
        /// <summary>
        /// Id of strategy
        /// </summary>
        public int Id { get; set; }
    }
}
