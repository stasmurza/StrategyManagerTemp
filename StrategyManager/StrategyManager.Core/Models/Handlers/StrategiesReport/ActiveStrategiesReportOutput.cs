using StrategyManager.Core.Models.DTOs.Reports;

namespace StrategyManager.Core.Models.Handlers.StrategiesReport
{
    public class ActiveStrategiesReportOutput
    {
        public IEnumerable<StrategyDTO> Strategies { get; set; } = Enumerable.Empty<StrategyDTO>();
    }
}
