namespace StrategyManager.Core.Models.Services.Strategies.Turtles
{
    public enum StrategyStep
    {
        ListeningEntrySignal,
        CreatingEntryPendingOrder,
        HandlingEntryOrder,
        ListeningExitSignal,
        CreatingExitPendingOrder,
        HandlingExitOrder,
    }
}
