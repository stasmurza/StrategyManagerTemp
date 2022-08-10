using StrategyManager.Core.Models.Services.Strategies;

namespace StrategyManager.Core.Services.Abstractions
{
    public interface IStrategy : IDisposable
    {
        StrategyStatus Status { get; }
        StrategyCode Code { get; }
        string TicketCode { get; }
        DateTime LastActive { get; }
        Task StartAsync(string ticketCode, CancellationTokenSource cancellationTokenSource);
        Task StopAsync();
    }
}
