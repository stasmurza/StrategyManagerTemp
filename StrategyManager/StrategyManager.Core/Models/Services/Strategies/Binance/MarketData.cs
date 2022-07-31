namespace StrategyManager.Core.Models.Services.Jobs.Binance
{
    public struct MarketData
    {
        public string Symbol { get; set; }
        public string DateAndTime { get; set; }
        public string Side { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
    }
}
