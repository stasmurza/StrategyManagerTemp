namespace StrategyManager.Core.Models.Services.Strategies
{
    public class TicketStatus
    {
        public string StrategyCode { get; set; } = String.Empty;
        public string TicketCode { get; set; } = String.Empty;
        public string Status { get; set; } = String.Empty;
        public DateTime LastActive { get; set; } = default;
    }
}
