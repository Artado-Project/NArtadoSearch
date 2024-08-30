using NArtadoSearch.Entities.Dto;

namespace NArtadoSearch.Business.Services.Abstractions;

public interface IWebIndexService
{
    Task AddAsync(IndexWebUrlDto data);
    Task BulkAddAsync(IEnumerable<IndexWebUrlDto> data);
    Task DeleteAsync(int id);
    Task BulkDeleteAsync(IEnumerable<int> ids);
    Task UpdateAsync(IndexWebUrlDto data);
    Task SynchronizeAsync();
}