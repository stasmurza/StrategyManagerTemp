using StrategyManager.Core.Models.Services.Strategies;

namespace StrategyManager.Core.Services.Abstractions
{
    public interface IStrategyManager
    {
        void Start(string strategyCode, string ticketCode);
        Task StopAsync(string strategyCode, string ticketCode);
        IEnumerable<TicketStatus> GetTicketStatuses();
        TicketStatus GetTicketStatus(string strategyCode, string ticketCode);
    }
}
