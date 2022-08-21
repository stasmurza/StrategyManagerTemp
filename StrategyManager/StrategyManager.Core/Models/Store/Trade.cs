namespace StrategyManager.Core.Models.Store
{
    public class Trade : Entity
    {
        public string Code { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public Order? Order { get; set; }
        public string OrderId { get; set; }
    }
}
