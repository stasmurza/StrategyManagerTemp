namespace StrategyManager.Core.Models.Services.Strategies.Turtles
{
    public class PendingOrderInput
    {
        public string StrategyId { get; private set; }
        public string InstrumentCode { get; private set; }
        public Direction Direction { get; private set; }
        public decimal Volume { get; private set; }
        public decimal Price { get; private set; }

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
