using MediatR;

namespace StrategyManager.Core.Models.Handlers.Strategies.Tickets
{
    public class RemoveTicketInput : IRequest<Unit>
    {
        public int StrategyId { get; set; }
        public string Code { get; set; } = String.Empty;
    }
}
