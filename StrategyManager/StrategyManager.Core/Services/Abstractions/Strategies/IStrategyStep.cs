namespace StrategyManager.Core.Services.Abstractions.Strategies
{
    public interface IIdempotentStep
    {
        public event EventHandler<EventArgs>? Start;
        public event EventHandler<EventArgs>? Success;
        public event EventHandler<EventArgs>? Error;
        public IIdempotentStep? Next { get; set; }

        /// <summary>
        /// Idempotent run
        /// </summary>
        public void Run();

        /// <summary>
        /// Idempotent stop
        /// </summary>
        public void Stop();
    }
}
