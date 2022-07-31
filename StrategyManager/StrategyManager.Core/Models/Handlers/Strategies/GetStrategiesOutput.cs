using StrategyManager.Core.Models.DTOs;

namespace StrategyManager.Core.Models.Handlers.Strategies
{
    public class GetStrategiesOutput
    {
        public IEnumerable<StrategyDTO> Jobs { get; set; } = Enumerable.Empty<StrategyDTO>();
    }
}
