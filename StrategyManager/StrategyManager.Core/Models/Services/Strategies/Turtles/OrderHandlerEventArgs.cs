using StrategyManager.Core.Models.DTOs.Strategies;

namespace StrategyManager.Core.Models.Services.Strategies.Turtles
{
    public class OrderHandlerEventArgs
    {
        public OrderDTO Order { get; private set; }

        public OrderHandlerEventArgs(OrderDTO order)
        {
            Order = order;
        }
    }
}
