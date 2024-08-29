using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NArtadoSearch.Core.DataAccess.Abstractions;
using NArtadoSearch.Core.Entities;

namespace NArtadoSearch.Core.DataAccess.EntityFramework;

public class EfGenericReadRepository<TEntity>(DbContext dbContext) : IGenericReadRepository<TEntity>
    where TEntity : class, IEntity, new()
{
    public IEnumerable<TEntity> GetAll()
    {
        return dbContext.Set<TEntity>().AsEnumerable();
    }

    public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression)
    {
        return dbContext.Set<TEntity>().Where(expression).AsEnumerable();
    }

    public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, object>> orderBy, bool orderByDescending = false)
    {
        var filter = dbContext.Set<TEntity>().Where(expression);
        filter = orderByDescending ? filter.OrderByDescending(orderBy) : filter.OrderBy(orderBy);
        return filter.AsEnumerable();
    }

    public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, object>> orderBy, int pageNumber, int pageSize, bool orderByDescending = false)
    {
        var filter = dbContext.Set<TEntity>().Where(expression);
        filter = orderByDescending ? filter.OrderByDescending(orderBy) : filter.OrderBy(orderBy);
        filter = filter.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        return filter.AsEnumerable();
    }

    public async Task<TEntity?> GetSingle(Expression<Func<TEntity, bool>> expression)
    {
        return await dbContext.Set<TEntity>().FirstAsync(expression);
    }

    public async Task<int> Count(Expression<Func<TEntity, bool>>? expression = null)
    {
        return expression != null
            ? await dbContext.Set<TEntity>().CountAsync(expression)
            : await dbContext.Set<TEntity>().CountAsync();
    }
}