using StrategyManager.Core.Models.Store;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace StrategyManager.Core.Repositories.Abstractions
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        Task CreateAsync(TEntity entity);
        Task<ReplaceOneResult> UpdateAsync(TEntity entity);
        Task<DeleteResult> DeleteAsync(string id);
        Task<TEntity> GetByIdAsync(string id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, string order);
    }
}
