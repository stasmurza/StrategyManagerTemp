using MediatR;

namespace StrategyManager.Core.Models.Handlers.Strategies
{
    public class StopStrategyInput : IRequest<Unit>
    {
        public string Id { get; set; } = String.Empty;
    }
}
