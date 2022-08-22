using StrategyManager.Core.Models.DTOs.Strategies;

namespace StrategyManager.Core.Models.Services.Strategies.Turtles
{
    public class PendingOrderEventArgs
    {
        public string StrategyId { get; private set; }
        public string OrderGuid { get; private set; }

        public PendingOrderEventArgs(string strategyId, string orderGuid)
        {
            StrategyId = strategyId;
            OrderGuid = orderGuid;
        }
    }
}
