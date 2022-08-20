namespace StrategyManager.Core.Models.Services.Strategies.Turtles
{
    public class PendingOrderInput
    {
        public string StrategyId { get; set; }
        public string InstrumentCode { get; set; }
        public Direction Direction { get; set; }
        public decimal Volume { get; set; }
        public decimal Price { get; set; }

        public PendingOrderInput(
            string strategyId,
            string instrumentCode,
            Direction direction,
            decimal volume,
            decimal price)
        {
            StrategyId = strategyId;
            InstrumentCode = instrumentCode;
            Direction = direction;
            Volume = volume;
            Price = price;
        }
    }
}
