using MediatR;

namespace StrategyManager.Core.Models.Handlers.Strategies.Tickets
{
    public class RemoveTicketInput : IRequest<Unit>
    {
        public string JobId { get; set; } = String.Empty;
        public string Code { get; set; } = String.Empty;
    }
}
