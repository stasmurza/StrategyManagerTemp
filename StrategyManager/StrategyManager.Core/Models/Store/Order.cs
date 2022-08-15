using StrategyManager.Core.Models.Services.Strategies;

namespace StrategyManager.Core.Models.Store
{
    public class Order : IEntity
    {
        public string Id { get; set; }
        public string InstrumentCode { get; set; } = String.Empty;
        public string StrategyId { get; set; } = String.Empty;
        public PositionDirection Direction { get; set; }
        public decimal Volume { get; set; }
        public decimal Price { get; set; }
        public DateTime DateTime { get; set; }
    }
}
