using StrategyManager.Core.Models.Store;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace StrategyManager.Core.Repositories.Abstractions
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        void RemoveRange(TEntity[] entities);
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity?> FirstOrDefaultAsync<TOrderBy>(Expression<Func<TEntity, bool>>? wherePredicate = null, Expression<Func<TEntity, TOrderBy>>? orderExpression = null, bool ascending = true, Expression<Func<TEntity, object>>[]? includes = null);
    }
}
