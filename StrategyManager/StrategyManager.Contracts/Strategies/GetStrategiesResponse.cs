﻿namespace StrategyManager.Contracts.Strategies
{
    /// <summary>
    /// Get strategies response
    /// </summary>
    public class GetStrategiesResponse
    {
        /// <summary>
        /// Strategies
        /// </summary>
        public IEnumerable<StrategyViewModel> Jobs { get; set; } = Enumerable.Empty<StrategyViewModel>();
    }
}
