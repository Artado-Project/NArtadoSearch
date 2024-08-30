using NArtadoSearch.Core.Entities;

namespace NArtadoSearch.Core.DataAccess.ElasticSearch.Abstractions;

public interface IQueryService<TEntity> where TEntity : class, IEntity, new()
{
    Task<TEntity> GetByIdAsync(int id);
    Task<IEnumerable<TEntity>> SearchAsync(string query);
}