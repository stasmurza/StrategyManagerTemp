using StrategyManager.Core.Models.Services.Strategies;

namespace StrategyManager.Core.Models.Store
{
    public class Order : Entity
    {
        public string InstrumentCode { get; set; } = String.Empty;
        public string StrategyId { get; set; } = String.Empty;
        public PositionDirection Direction { get; set; }
        public decimal Volume { get; set; }
        public decimal Price { get; set; }
        public DateTime DateTime { get; set; }
        public ICollection<Trade> Tickets { get; set; } = new List<Trade>();
    }
}
