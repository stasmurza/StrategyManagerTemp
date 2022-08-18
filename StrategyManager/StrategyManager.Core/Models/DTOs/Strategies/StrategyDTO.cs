using StrategyManager.Core.Models.Store;

namespace StrategyManager.Core.Models.DTOs.Strategies
{
    public class StrategyDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool StartWithService { get; set; } = false;
        public IList<TicketDTO> Tickets { get; set; } = new List<TicketDTO>();
    }
}
