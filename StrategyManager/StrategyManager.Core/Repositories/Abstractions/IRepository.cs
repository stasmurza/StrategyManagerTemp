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
        Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>>? wherePredicate = null,
            Expression<Func<TEntity, object>>[]? includes = null);
        Task<TEntity?> FirstOrDefaultAsync<TOrderBy>(
            Expression<Func<TEntity, TOrderBy>> orderExpression,
            bool ascending = true,
            Expression<Func<TEntity, bool>>? wherePredicate = null,
            Expression<Func<TEntity, object>>[]? includes = null);
        Task<List<TEntity>> GetAllAsync(int skip = 0, int top = int.MaxValue, Expression<Func<TEntity, object>>[]? includes = null);
    }
}
