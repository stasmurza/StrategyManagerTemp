namespace StrategyManager.Core.Models.DTOs.Reports
{
    public class StrategyDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public IList<TicketDTO> Tickets { get; set; } = new List<TicketDTO>();
    }
}
