namespace StrategyManager.Contracts.Strategies
{
    /// <summary>
    /// Get active jobs response
    /// </summary>
    public class GetActiveStrategiesResponse
    {
        /// <summary>
        /// Active jobs
        /// </summary>
        public IEnumerable<JobViewModel> Jobs { get; set; } = Enumerable.Empty<JobViewModel>();
    }
}
