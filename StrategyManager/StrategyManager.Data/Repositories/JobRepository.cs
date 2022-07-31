using StrategyManager.Core.Models.Store;

namespace StrategyManager.Data.Repositories
{
    public class JobRepository : RepositoryBase<Strategy>
    {
        public JobRepository(IEventStoreDbContext dbContext) : base(dbContext.Jobs) { }
    }
}
