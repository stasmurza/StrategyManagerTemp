namespace StrategyManager.Contracts.Strategies
{
    /// <summary>
    /// Get strategies response
    /// </summary>
    public class GetStrategiesResponse
    {
        /// <summary>
        /// Strategies
        /// </summary>
        public IEnumerable<StrategyViewModel> Strategies { get; set; } = Enumerable.Empty<StrategyViewModel>();
    }
}
