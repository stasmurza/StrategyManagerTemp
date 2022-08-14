namespace StrategyManager.Core.Models.Services.Strategies.Turtles
{
    public class OrderHandlerInput
    {
        public string OrderId { get; set; } = string.Empty;
        public string InstrumentCode { get; set; } = string.Empty;
        public PositionDirection Direction { get; set; }
        public decimal Volume { get; set; }
    }
}
