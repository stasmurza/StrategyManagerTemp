namespace StrategyManager.Core.Models.Services.Strategies
{
    public class Position
    {
        public PositionDirection Direction { get; private set; }
        public int Count { get; private  set; }

        public Position(PositionDirection direction, int count)
        {
            Direction = direction;
            Count = count;
        }
    }
}
