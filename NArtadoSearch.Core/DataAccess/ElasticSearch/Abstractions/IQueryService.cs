using NArtadoSearch.Core.Entities;

namespace NArtadoSearch.Core.DataAccess.ElasticSearch.Abstractions;

public interface IQueryService<T> where T : class, new()
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> SearchAsync(string query, int page = 1, int pageSize = 20);
    Task PurgeAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
}