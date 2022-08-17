using StrategyManager.Core.Models.Store;

namespace StrategyManager.Data.Repositories
{
    public class StrategyRepository : RepositoryBase<Strategy>
    {
        public StrategyRepository(StrategyManagerDbContext dbContext) : base(dbContext) { }
    }
}
