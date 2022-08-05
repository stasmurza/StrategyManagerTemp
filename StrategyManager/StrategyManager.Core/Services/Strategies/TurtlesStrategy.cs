using StrategyManager.Core.Models.Services.Strategies;
using StrategyManager.Core.Services.Abstractions.Strategies;

namespace StrategyManager.Core.Services.Strategies
{
    public class TurtlesStrategy : ITurtlesStrategy
    {
        public StrategyStatus Status { get; private set; } = StrategyStatus.Stopped;

        public StrategyCode Code { get; private set; } = default;

        public string TicketCode { get; private set; } = String.Empty;

        public DateTime LastActive { get; private set; } = default;

        public TurtlesStrategy(
            StrategyCode code,
            string ticketCode)
        {
            Code = code;
            TicketCode = ticketCode;
        }

        public void Dispose()
        {
        }

        public async Task StartAsync(CancellationTokenSource cancellationTokenSource)
        {
            Status = StrategyStatus.Running;
        }

        public async Task StopAsync()
        {
            Status = StrategyStatus.Stopped;
        }
    }
}
