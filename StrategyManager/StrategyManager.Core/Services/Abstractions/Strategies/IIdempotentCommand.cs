namespace StrategyManager.Core.Services.Abstractions.Strategies
{
    public interface IIdempotentCommand
    {
        public void Stop();
        public event EventHandler<EventArgs>? Started;
        public event EventHandler<EventArgs>? Stopped;
    }
}
