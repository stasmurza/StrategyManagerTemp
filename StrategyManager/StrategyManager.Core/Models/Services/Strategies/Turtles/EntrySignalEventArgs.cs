namespace StrategyManager.Core.Models.Services.Strategies.Turtles
{
    public class EntrySignalEventArgs : EventArgs
    {
        public Direction Direction { get; set; }
        public decimal Price { get; set; }

        public EntrySignalEventArgs(Direction direction, decimal price)
        {
            Direction = direction;
            Price = price;
        }
    }
}
