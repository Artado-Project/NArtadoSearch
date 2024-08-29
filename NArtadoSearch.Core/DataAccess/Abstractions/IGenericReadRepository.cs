using System.Linq.Expressions;
using NArtadoSearch.Core.Entities;

namespace NArtadoSearch.Core.DataAccess.Abstractions;

public interface IGenericReadRepository<TEntity> where TEntity : class, IEntity, new()
{
    IEnumerable<TEntity> GetAll();
    IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression);

    IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, object>> orderBy, bool orderByDescending = false);

    IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression,
        Expression<Func<TEntity, object>> orderBy, int pageNumber, int pageSize, bool orderByDescending = false);

    Task<TEntity?> GetSingle(Expression<Func<TEntity, bool>> expression);
    Task<int> Count(Expression<Func<TEntity, bool>>? expression = null);
}