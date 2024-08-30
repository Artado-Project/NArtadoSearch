using Microsoft.EntityFrameworkCore;
using NArtadoSearch.Core.DataAccess.Abstractions;
using NArtadoSearch.Core.DataAccess.EntityFramework;
using NArtadoSearch.DataAccess.Abstractions.Read;
using NArtadoSearch.Entities.Concrete;

namespace NArtadoSearch.DataAccess.EntityFramework.Read;

public class EfIndexedWebUrlReadRepository(DbContext dbContext)
    : EfGenericReadRepository<IndexedWebUrl>(dbContext), IIndexedWebUrlReadRepository
{
}