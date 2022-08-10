namespace StrategyManager.Core.Models.Services.Strategies
{
    public class NewPositionEventArgs : EventArgs
    {
        public PositionDirection Direction { get; private set; }

        public NewPositionEventArgs(PositionDirection direction)
        {
            Direction = direction;
        }
    }
}
