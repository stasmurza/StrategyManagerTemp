namespace StrategyManager.Data
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
