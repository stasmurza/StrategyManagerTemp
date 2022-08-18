using MediatR;

namespace StrategyManager.Core.Models.Handlers.Strategies
{
    public class StopStrategyInput : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
