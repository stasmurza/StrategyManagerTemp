namespace StrategyManager.Core.Models.Services.Strategies
{
    public class StrategyState
    {
        public Position? Position { get; private set; }

        public StrategyState(Position? position)
        {
            Position = position;
        }
    }
}
