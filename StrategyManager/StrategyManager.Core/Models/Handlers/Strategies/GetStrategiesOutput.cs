using StrategyManager.Core.Models.DTOs.Strategies;

namespace StrategyManager.Core.Models.Handlers.Strategies
{
    public class GetStrategiesOutput
    {
        public IEnumerable<StrategyDTO> Strategies { get; set; } = Enumerable.Empty<StrategyDTO>();
    }
}
