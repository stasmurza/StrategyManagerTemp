namespace StrategyManager.Core.Services.Abstractions
{
    public interface IBackgroundService
    {
        public void StartJob();

        public Task StopJobAsync();
    }
}
