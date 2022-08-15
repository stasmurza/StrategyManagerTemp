namespace StrategyManager.Core.Models.Services.Strategies.Turtles
{
    public class PendingOrderInput
    {
        public string StrategyId { get; set; } = string.Empty;
        public string InstrumentCode { get; set; } = string.Empty;
        public PositionDirection Direction { get; set; }
        public decimal Volume { get; set; }
        public decimal Price { get; set; }
    }
}
