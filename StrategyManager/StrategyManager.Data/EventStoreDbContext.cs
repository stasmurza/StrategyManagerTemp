using StrategyManager.Core.Models.Store;
using StrategyManager.Core.Models.Store.Events;
using StrategyManager.Infrastructure.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace StrategyManager.Data
{
    public class EventStoreDbContext : IEventStoreDbContext, IDisposable
    {
        private readonly IMongoClient client;
        public IMongoDatabase Database { get; }
        private bool disposed = false;

        public EventStoreDbContext(IOptions<DatabaseOptions> options)
        {
            client = new MongoClient(options.Value.ConnectionString);
            Database = client.GetDatabase(options.Value.DatabaseName);
        }

        public IMongoCollection<Event> Events => Database.GetCollection<Event>(CollectionNames.Events);
        public IMongoCollection<Strategy> Jobs => Database.GetCollection<Strategy>(CollectionNames.Jobs);

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return Database.GetCollection<T>(name);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed) return;

            // If disposing equals true, dispose all managed
            // and unmanaged resources.
            if (disposing)
            {
                // Dispose managed resources.
            }

            // Call the appropriate methods to clean up
            // unmanaged resources here.
            client.Cluster.Dispose();

            // Note disposing has been done.
            disposed = true;
        }

        ~EventStoreDbContext()
        {
            Dispose(false);
        }

    }
}