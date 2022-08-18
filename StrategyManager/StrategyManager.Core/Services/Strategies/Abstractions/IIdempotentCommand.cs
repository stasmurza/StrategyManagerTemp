namespace StrategyManager.Core.Services.Strategies.Abstractions
{
    public interface IIdempotentCommand
    {
        public void Stop();
        public event EventHandler<EventArgs>? Started;
        public event EventHandler<EventArgs>? Stopped;
    }
}
