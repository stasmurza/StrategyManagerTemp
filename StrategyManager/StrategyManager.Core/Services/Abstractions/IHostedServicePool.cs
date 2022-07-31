using StrategyManager.Core.Models.Services.Jobs;

namespace StrategyManager.Core.Services.Abstractions
{
    public interface IHostedServicePool
    {
        IBackgroundService GetHostedServiceByJobCode(string code);
    }
}
