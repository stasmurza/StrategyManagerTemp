namespace StrategyManager.Core.Models.Services.Strategies.Turtles
{
    public class ExitSignalInput
    {
        public string InstrumentCode { get; set; } = string.Empty;
        public PositionDirection Direction { get; set; }
    }
}
