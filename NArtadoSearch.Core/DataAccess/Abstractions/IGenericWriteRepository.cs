using NArtadoSearch.Core.Entities;

namespace NArtadoSearch.Core.DataAccess.Abstractions;

public interface IGenericWriteRepository<TEntity> where TEntity : class, IEntity, new()
{
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    Task UpdateAsync(TEntity entity);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities);
    Task DeleteAsync(TEntity entity);
    Task DeleteRangeAsync(IEnumerable<TEntity> entities);
    Task DeleteRangeAsync(IEnumerable<int> ids);
    Task DeleteAsync(int id);
}