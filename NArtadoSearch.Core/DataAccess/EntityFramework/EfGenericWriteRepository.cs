using Microsoft.EntityFrameworkCore;
using NArtadoSearch.Core.DataAccess.Abstractions;
using NArtadoSearch.Core.Entities;

namespace NArtadoSearch.Core.DataAccess.EntityFramework;

public class EfGenericWriteRepository<TEntity>(DbContext dbContext) : IGenericWriteRepository<TEntity>
    where TEntity : class, IEntity, new()
{
    public async Task AddAsync(TEntity entity)
    {
        await dbContext.Set<TEntity>().AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await dbContext.Set<TEntity>().AddRangeAsync(entities);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        dbContext.Set<TEntity>().Update(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        dbContext.Set<TEntity>().UpdateRange(entities);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        dbContext.Set<TEntity>().Remove(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
    {
        dbContext.Set<TEntity>().RemoveRange(entities);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(IEnumerable<int> ids)
    {
        dbContext.Set<TEntity>().RemoveRange(ids.Select(x => new TEntity() { Id = x }));
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        dbContext.Set<TEntity>().Remove(new TEntity() { Id = id });
        await dbContext.SaveChangesAsync();
    }
}