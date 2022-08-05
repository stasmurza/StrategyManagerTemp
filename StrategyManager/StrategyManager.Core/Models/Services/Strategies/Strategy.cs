namespace StrategyManager.Core.Models.Services.Strategies
{
    public class Strategy
    {
        public StrategyCode StrategyCode { get; set; } = default;
        public string TicketCode { get; set; } = string.Empty;
        public StrategyStatus Status { get; set; } = StrategyStatus.Stopped;
    }
}
