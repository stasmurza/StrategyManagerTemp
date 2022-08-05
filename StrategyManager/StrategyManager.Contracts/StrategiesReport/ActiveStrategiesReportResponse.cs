namespace StrategyManager.Contracts.StrategiesReport
{
    /// <summary>
    /// Active strategies report response
    /// </summary>
    public class ActiveStrategiesReportResponse
    {
        /// <summary>
        /// Active strategies
        /// </summary>
        public IEnumerable<StrategyViewModel> Jobs { get; set; } = Enumerable.Empty<StrategyViewModel>();
    }
}
