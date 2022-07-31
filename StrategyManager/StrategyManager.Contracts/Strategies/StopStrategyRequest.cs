namespace StrategyManager.Contracts.Strategies
{
    /// <summary>
    /// Stop job by id
    /// </summary>
    public class StopStrategyRequest
    {
        /// <summary>
        /// Id of job
        /// </summary>
        public string Id { get; set; } = String.Empty;
    }
}
