namespace StrategyManager.Core.Models.Services
{
    public struct MarketData
    {
        public string Symbol { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Side { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
    }
}
