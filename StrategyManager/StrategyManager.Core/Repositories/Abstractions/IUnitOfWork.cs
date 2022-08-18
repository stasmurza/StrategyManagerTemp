namespace StrategyManager.Core.Repositories.Abstractions
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
