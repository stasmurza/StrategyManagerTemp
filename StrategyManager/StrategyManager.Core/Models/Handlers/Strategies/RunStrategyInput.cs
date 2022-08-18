using MediatR;

namespace StrategyManager.Core.Models.Handlers.Strategies
{
    public class RunStrategyInput : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
