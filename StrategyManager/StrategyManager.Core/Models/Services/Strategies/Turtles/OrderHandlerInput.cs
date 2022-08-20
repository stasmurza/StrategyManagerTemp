using StrategyManager.Core.Models.DTOs.Strategies;
using StrategyManager.Core.Models.Store;

namespace StrategyManager.Core.Models.Services.Strategies.Turtles
{
    public class OrderHandlerInput
    {
        public string StrategyId { get; set; } = String.Empty;
        public OrderDTO Order { get; set; }

        public OrderHandlerInput(string strategyId, OrderDTO order)
        {
            this.StrategyId = strategyId;
            this.Order = order;
        }
    }
}
