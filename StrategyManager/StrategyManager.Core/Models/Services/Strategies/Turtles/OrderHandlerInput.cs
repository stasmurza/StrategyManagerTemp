namespace StrategyManager.Core.Models.Services.Strategies.Turtles
{
    public class OrderHandlerInput
    {
        public string StrategyId { get; private set; }
        public string OrderGuid { get; private set; }

        public OrderHandlerInput(string strategyId, string orderGuid)
        {
            this.StrategyId = strategyId;
            OrderGuid = orderGuid;
        }
    }
}
