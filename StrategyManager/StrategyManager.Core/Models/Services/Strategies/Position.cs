namespace StrategyManager.Core.Models.Services.Strategies
{
    public class Position
    {
        public Direction Direction { get; private set; }
        public int Count { get; private  set; }

        public Position(Direction direction, int count)
        {
            Direction = direction;
            Count = count;
        }
    }
}
