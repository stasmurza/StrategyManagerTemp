namespace StrategyManager.Contracts.Strategies
{
    /// <summary>
    /// Start job by id
    /// </summary>
    public class StartStrategyRequest
    {
        /// <summary>
        /// Id of job
        /// </summary>
        public string Id { get; set; } = String.Empty;
    }
}
