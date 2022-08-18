using MediatR;

namespace StrategyManager.Core.Models.Handlers.Strategies.Tickets
{
    public class AddTicketInput : IRequest<Unit>
    {
        public int StrategyId { get; set; }
        public string Code { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
    }
}
