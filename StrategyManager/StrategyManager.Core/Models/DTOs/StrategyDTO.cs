using StrategyManager.Core.Models.Store;

namespace StrategyManager.Core.Models.DTOs
{
    public class StrategyDTO
    {
        public string Id { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public bool StartWithService { get; set; } = false;
        public IList<TicketDTO> Tickets { get; set; } = new List<TicketDTO>();
    }
}
