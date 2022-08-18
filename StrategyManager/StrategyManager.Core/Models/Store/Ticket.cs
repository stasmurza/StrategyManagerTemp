﻿namespace StrategyManager.Core.Models.Store
{
    public class Ticket : Entity
    {
        public string Code { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public Strategy? Strategy { get; set; }
        public int StrategyId { get; set; }
    }
}
