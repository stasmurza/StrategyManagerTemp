using StrategyManager.Core.Models.Store;
using StrategyManager.Core.Models.Store.Events;

namespace StrategyManager.Data.Repositories
{
    public class OrderRepository : RepositoryBase<Order>
    {
        public OrderRepository(StrategyManagerDbContext dbContext) : base(dbContext) { }
    }
}
