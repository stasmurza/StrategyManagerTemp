using MediatR;

namespace StrategyManager.Core.Models.Handlers.Strategies
{
    public class RunStrategyInput : IRequest<Unit>
    {
        public string Id { get; set; } = String.Empty;
    }
}
