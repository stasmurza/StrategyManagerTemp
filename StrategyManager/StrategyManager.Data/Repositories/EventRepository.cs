using StrategyManager.Core.Models.Store.Events;

namespace StrategyManager.Data.Repositories
{
    public class EventRepository : RepositoryBase<Event>
    {
        public EventRepository(StrategyManagerDbContext dbContext) : base(dbContext) { }
    }
}
