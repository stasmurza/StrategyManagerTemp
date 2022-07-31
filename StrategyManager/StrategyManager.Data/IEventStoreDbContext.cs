using StrategyManager.Core.Models.Store;
using StrategyManager.Core.Models.Store.Events;
using MongoDB.Driver;

namespace StrategyManager.Data
{
    public interface IEventStoreDbContext
    {
        IMongoDatabase Database { get; }
        IMongoCollection<Event> Events { get; }
        IMongoCollection<Strategy> Jobs { get; }

        public IMongoCollection<T> GetCollection<T>(string name);
    }
}
