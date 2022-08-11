using Microsoft.Extensions.Options;
using StrategyManager.Core.Models.Options;
using StrategyManager.Core.Models.Services.MarketDataProvider;
using StrategyManager.Core.Models.Services.Strategies;
using StrategyManager.Core.Services.Abstractions;
using StrategyManager.Core.Services.Abstractions.Strategies;

namespace StrategyManager.Core.Services.Strategies.Turtles
{
    //read 
    // position opening is blocking synchronous operation
    public class TurtlesStrategy : ITurtlesStrategy
    {
        public StrategyStatus Status { get; private set; } = StrategyStatus.Stopped;

        public StrategyCode Code { get; private set; } = StrategyCode.Turtles;

        public string TicketCode { get; private set; } = string.Empty;

        public DateTime LastActive { get; private set; } = default;

        private readonly TurtlesStrategySaga strategySaga;
        private readonly IStrategyStateProvider stateProvider;
        private readonly IMarketDataProvider marketDataProvider;
        private IEntryRules entryRules;
        private IExitRules exitRules;
        private Position? activePosition;
        private bool disposed;
        private EventHandler<ClosePositionEventArgs>? CloseSignalHandler;
        private EventHandler<NewPositionEventArgs>? OpenSignalHandler;
        private EventHandler<MarketDataEventArgs>? MarketDataHandler;

        public TurtlesStrategy(
            IStrategyStateProvider stateProvider,
            IMarketDataProvider marketDataProvider,
            IEntryRules entryRules,
            IExitRules exitRules)
        {
            this.stateProvider = stateProvider;
            this.marketDataProvider = marketDataProvider;
            this.entryRules = entryRules;
            this.exitRules = exitRules;

            exitRules.NewSignal += CloseSignalHandler;
            entryRules.NewSignal += OpenSignalHandler;
            marketDataProvider.PriceChanged += MarketDataHandler;
        }

        public async Task StartAsync(string ticketCode, CancellationTokenSource cancellationTokenSource)
        {
            TicketCode = ticketCode;
            Status = StrategyStatus.Starting;

            strategySaga.Start();

            //Check orders
            CloseSignalHandler += ExitRules_NewSignal;
            OpenSignalHandler += EntryRules_NewSignal;
            MarketDataHandler += MarketDataProvider_PriceChanged;
        }

        public async Task StopAsync()
        {
            Status = StrategyStatus.Stopping;
            strategySaga.Stop();
            CloseSignalHandler -= ExitRules_NewSignal;
            OpenSignalHandler -= EntryRules_NewSignal;
            MarketDataHandler -= MarketDataProvider_PriceChanged;
            Status = StrategyStatus.Stopped;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                // TODO: dispose managed state (managed objects (resources)).
                exitRules.NewSignal -= CloseSignalHandler;
                entryRules.NewSignal -= OpenSignalHandler;
                marketDataProvider.PriceChanged -= MarketDataHandler;
                CloseSignalHandler = null;
                OpenSignalHandler = null;
                MarketDataHandler = null;
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            disposed = true;
        }

        private void ExitRules_NewSignal(object? sender, ClosePositionEventArgs e)
        {
            ClosePosition(e.Position);
        }

        private void EntryRules_NewSignal(object? sender, NewPositionEventArgs e)
        {
            OpenPosition();
        }

        private void MarketDataProvider_PriceChanged(object? sender, MarketDataEventArgs e)
        {
            this.entryRules.Process(e.MarketData, activePosition);
            this.exitRules.Process(e.MarketData, activePosition);
        }

        private void OpenPosition()
        {

        }

        private void ClosePosition(Position position)
        {
            activePosition = null;
        }

        ~TurtlesStrategy() => Dispose(false);
    }
}
