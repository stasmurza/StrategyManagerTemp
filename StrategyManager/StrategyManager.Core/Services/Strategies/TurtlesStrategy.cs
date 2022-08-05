using StrategyManager.Core.Models.Services.Strategies;
using StrategyManager.Core.Services.Abstractions;
using StrategyManager.Core.Services.Abstractions.Strategies;

namespace StrategyManager.Core.Services.Strategies
{
    public class TurtlesStrategy : ITurtlesStrategy
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
