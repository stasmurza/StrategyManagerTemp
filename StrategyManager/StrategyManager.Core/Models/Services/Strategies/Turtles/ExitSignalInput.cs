using StrategyManager.Core.Models.DTOs.Strategies;
using StrategyManager.Core.Models.Store;

namespace StrategyManager.Core.Models.Services.Strategies.Turtles
{
    public class ExitSignalInput
    {
        public string StrategyId { get; private set; }
        public string InstrumentCode { get; private set; }
        public OrderDTO EntryOrder { get; private set; }

        public ExitSignalInput(string strategyId, string instrumentCode, OrderDTO entryOrder)
        {
            StrategyId = strategyId;
            InstrumentCode = instrumentCode;
            EntryOrder = entryOrder;
        }
    }
}
