using StrategyManager.Core.Models.Services.Strategies;
using StrategyManager.Core.Services.Abstractions;

namespace StrategyManager.Core.Services.Strategies
{
    public class TurtlesStrategy : IStrategy
    {
        public StrategyStatus Status => throw new NotImplementedException();

        public StrategyCode Code => throw new NotImplementedException();

        public string TicketCode => throw new NotImplementedException();

        public DateTime LastActive => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task StartAsync(CancellationTokenSource cancellationTokenSource)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync()
        {
            throw new NotImplementedException();
        }
    }
}
