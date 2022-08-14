namespace StrategyManager.Core.Models.Services.Strategies.Turtles
{
    public class ExitSignalEventArgs : EventArgs
    {
        public PositionDirection Direction { get; set; }
    }
}
