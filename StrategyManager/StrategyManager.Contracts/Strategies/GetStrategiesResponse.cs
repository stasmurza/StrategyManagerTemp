namespace StrategyManager.Contracts.Strategies
{
    /// <summary>
    /// Get jobs response
    /// </summary>
    public class GetStrategiesResponse
    {
        /// <summary>
        /// Jobs
        /// </summary>
        public IEnumerable<JobViewModel> Jobs { get; set; } = Enumerable.Empty<JobViewModel>();
    }
}
