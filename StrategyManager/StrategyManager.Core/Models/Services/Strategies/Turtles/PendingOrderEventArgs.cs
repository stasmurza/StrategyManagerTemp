using StrategyManager.Core.Models.DTOs.Strategies;

namespace StrategyManager.Core.Models.Services.Strategies.Turtles
{
    public class PendingOrderEventArgs
    {
        public string StrategyId { get; set; }
        public OrderDTO Order { get; set; }

        public PendingOrderEventArgs(string strategyId, OrderDTO order)
        {
            StrategyId = strategyId;
            Order = order;
        }
    }
}
