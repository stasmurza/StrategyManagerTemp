using StrategyManager.Core.Models.Store;
using StrategyManager.Core.Repositories.Abstractions;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace StrategyManager.Data.Repositories
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly DbContext dbContext;

        public RepositoryBase(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(TEntity entity)
        {
            await dbContext.Set<TEntity>().AddAsync(entity);
        }

        public Task<TEntity?> GetByIdAsync(int id)
        {
            return dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>>? wherePredicate = null,
            Expression<Func<TEntity, object>>[]? includes = null)
        {
            var query = dbContext.Set<TEntity>().AsQueryable();

            if (wherePredicate != null) query = query.Where(wherePredicate);

            if (includes != null && includes.Any())
            {
                query = includes.Aggregate(query, (q, include) => q.Include(include));
            }

            var result = await query.FirstOrDefaultAsync();

            return result;
        }

        public async Task<TEntity?> FirstOrDefaultAsync<TOrderBy>(
            Expression<Func<TEntity, TOrderBy>> orderExpression,
            bool ascending = true,
            Expression<Func<TEntity, bool>>? wherePredicate = null,
            Expression<Func<TEntity, object>>[]? includes = null)
        {
            var query = dbContext.Set<TEntity>().AsQueryable();

            if (wherePredicate != null) query = query.Where(wherePredicate);
            if (orderExpression != null)
            {
                if (ascending) query = query.OrderBy(orderExpression);
                else query = query.OrderByDescending(orderExpression);
            }

            if (includes != null && includes.Any())
            {
                query = includes.Aggregate(query, (q, include) => q.Include(include));
            }

            var result = await query.FirstOrDefaultAsync();

            return result;
        }

        public void Remove(TEntity entity)
        {
            dbContext.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(TEntity[] entity)
        {
            dbContext.Set<TEntity>().RemoveRange(entity);
        }

        public void Update(TEntity entity)
        {
            dbContext.Set<TEntity>().Update(entity);
        }

        public Task<List<TEntity>> GetAllAsync(int skip = 0, int top = int.MaxValue, Expression<Func<TEntity, object>>[]? includes = null)
        {
            var query = dbContext.Set<TEntity>().AsQueryable();

            if (includes != null && includes.Any())
            {
                query = includes.Aggregate(query, (q, include) => q.Include(include));
            }

            return query
                .Skip(skip)
                .Take(top)
                .ToListAsync();
        }

        private Expression<Func<TEntity, TOrder>> GetOrderExpression<TOrder>(string sortField)
        {
            var property = typeof(TEntity).GetProperty(sortField);
            if (property == null) throw new ArgumentException($"Property {sortField} is not found");
            var orderByParameter = Expression.Parameter(typeof(TEntity));
            MemberExpression em = Expression.Property(orderByParameter, property);
            return Expression.Lambda<Func<TEntity, TOrder>>(Expression.Convert(em, typeof(TOrder)), orderByParameter);
        }
    }
}
