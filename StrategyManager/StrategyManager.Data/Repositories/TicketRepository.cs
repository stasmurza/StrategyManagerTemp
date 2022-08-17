using StrategyManager.Core.Models.Store;

namespace StrategyManager.Data.Repositories
{
    public class TicketRepository : RepositoryBase<Ticket>
    {
        public TicketRepository(StrategyManagerDbContext dbContext) : base(dbContext) { }
    }
}
