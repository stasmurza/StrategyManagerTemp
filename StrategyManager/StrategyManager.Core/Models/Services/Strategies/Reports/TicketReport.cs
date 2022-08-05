namespace StrategyManager.Core.Models.Services.Strategies.Reports
{
    public class TicketReport
    {
        public string StrategyCode { get; set; } = string.Empty;
        public string TicketCode { get; set; } = string.Empty;
        public StrategyStatus Status { get; set; } = StrategyStatus.Stopped;
        public DateTime LastActive { get; set; } = default;
    }
}
