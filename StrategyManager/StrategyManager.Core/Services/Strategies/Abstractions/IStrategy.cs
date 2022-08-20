using StrategyManager.Core.Models.Services.Strategies;

namespace StrategyManager.Core.Services.Strategies.Abstractions
{
    public interface IStrategy : IDisposable
    {
        public const string StartegyIdPattern = "{0}|{1}";
        StrategyStatus Status { get; }
        StrategyCode StrategyCode { get; }
        string InstrumentCode { get; }
        DateTime LastActive { get; }
        Task StartAsync(string ticketCode, CancellationTokenSource cancellationTokenSource);
        Task StopAsync();
        event EventHandler<NewStatusEventArgs>? OnStatusChange;
        public String StrategyId
        {
            get
            {
                return $"{StrategyCode}|{InstrumentCode}";
            }
        }
    }
}
