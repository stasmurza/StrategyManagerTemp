using StrategyManager.Core.Repositories.Abstractions;

namespace StrategyManager.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly StrategyManagerDbContext strategyManagerDbContext;

        public UnitOfWork(StrategyManagerDbContext strategyManagerDbContext)
        {
            this.strategyManagerDbContext = strategyManagerDbContext;
        }

        public Task CompleteAsync()
        {
            return strategyManagerDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            strategyManagerDbContext.Dispose();
        }
    }
}
