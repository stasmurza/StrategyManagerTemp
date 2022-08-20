using StrategyManager.Core.Models.Services.Strategies;

namespace StrategyManager.Core.Models.DTOs.Strategies
{
    public class OrderDTO
    {
        public string Id { get; set; } = String.Empty;
        public string InstrumentCode { get; set; } = String.Empty;
        public string StrategyId { get; set; } = String.Empty;
        public Direction Direction { get; set; }
        public decimal Volume { get; set; }
        public decimal Price { get; set; }
        public DateTime DateTime { get; set; }
    }
}
