using Microsoft.EntityFrameworkCore;
using NArtadoSearch.Core.DataAccess.Abstractions;
using NArtadoSearch.Core.DataAccess.EntityFramework;
using NArtadoSearch.DataAccess.Abstractions.Write;
using NArtadoSearch.Entities.Concrete;

namespace NArtadoSearch.DataAccess.EntityFramework.Write;

public class EfIndexedWebUrlWriteRepository(DbContext dbContext) : EfGenericWriteRepository<IndexedWebUrl>(dbContext),
    IIndexedWebUrlWriteRepository
{
}