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
        public IEnumerable<StrategyReportViewModel> Strategies { get; set; } = Enumerable.Empty<StrategyReportViewModel>();
    }
}
