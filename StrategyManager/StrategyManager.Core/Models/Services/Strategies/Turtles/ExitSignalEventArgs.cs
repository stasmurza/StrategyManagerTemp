using StrategyManager.Core.Models.DTOs.Strategies;

namespace StrategyManager.Core.Models.Services.Strategies.Turtles
{
    public class ExitSignalEventArgs : EventArgs
    {
        public Direction Direction { get; private set; }
        public decimal Volume { get; private set; }
        public decimal Price { get; private set; }

        public ExitSignalEventArgs(Direction direction, decimal volume, decimal price)
        {
            Direction = direction;
            Volume = volume;
            Price = price;
        }
    }
}
