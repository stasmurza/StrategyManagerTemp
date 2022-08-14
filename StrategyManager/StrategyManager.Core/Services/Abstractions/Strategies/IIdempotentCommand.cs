namespace StrategyManager.Core.Services.Abstractions.Strategies
{
    public interface IIdempotentCommand
    {
        public void Stop();
        public event EventHandler<EventArgs>? Start;
        public event EventHandler<EventArgs>? Finish;
        public event EventHandler<EventArgs>? Error;
    }
}
