using StrategyManager.Core.Models.Services.Strategies;

namespace StrategyManager.Core.Services.Strategies.Abstractions
{
    public interface IStrategy : IDisposable
    {
        StrategyStatus Status { get; }
        StrategyCode StrategyCode { get; }
        string InstrumentCode { get; }
        DateTime LastActive { get; }
        Task StartAsync(string ticketCode, CancellationTokenSource cancellationTokenSource);
        Task StopAsync();
    }
}
