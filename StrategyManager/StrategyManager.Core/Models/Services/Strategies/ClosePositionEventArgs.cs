namespace StrategyManager.Core.Models.Services.Strategies
{
    public class ClosePositionEventArgs : EventArgs
    {
        public Position Position { get; private set; }

        public ClosePositionEventArgs(Position position)
        {
            Position = position;
        }
    }
}
