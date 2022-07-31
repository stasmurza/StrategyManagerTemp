using StrategyManager.Core.Models.Store;
using StrategyManager.Core.Repositories.Abstractions;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace StrategyManager.Data.Repositories
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : IEntity
    {

        private readonly IMongoCollection<TEntity> collection;

        public RepositoryBase(IMongoCollection<TEntity> collection)
        {
            this.collection = collection;
        }

        public async Task CreateAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(typeof(TEntity).Name + " entity is null");
            await collection.InsertOneAsync(entity);
        }

        public async Task<ReplaceOneResult> UpdateAsync(TEntity entity)
        {
            var objectId = new ObjectId(entity.Id);
            var filter = Builders<TEntity>.Filter.Eq("_id", objectId);
            return await collection.ReplaceOneAsync(filter, entity);
        }

        public async Task<DeleteResult> DeleteAsync(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TEntity>.Filter.Eq("_id", objectId);
            return await collection.DeleteOneAsync(filter);

        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TEntity>.Filter.Eq("_id", objectId);
            var cursor = await collection.FindAsync(filter);
            return await cursor.FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var all = await collection.FindAsync(Builders<TEntity>.Filter.Empty);
            return await all.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            var result = await collection.FindAsync(expression);
            return await result.ToListAsync();
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
        {
            var options = new FindOptions<TEntity> { Limit = 1 };
            var cursor = await collection.FindAsync(expression, options);
            var list = await cursor.ToListAsync();
            return list.FirstOrDefault();
        }
    }
}
