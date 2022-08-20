﻿using StrategyManager.Core.Models.Services.Strategies;

namespace StrategyManager.Core.Models.Store
{
    public class Order : Entity
    {
        public new string Id { get; set; } = String.Empty;
        public string InstrumentCode { get; set; } = String.Empty;
        public string StrategyId { get; set; } = String.Empty;
        public Direction Direction { get; set; }
        public decimal Volume { get; set; }
        public decimal Price { get; set; }
        public DateTime DateTime { get; set; }
        public ICollection<Trade> Tickets { get; set; } = new List<Trade>();
    }
}
