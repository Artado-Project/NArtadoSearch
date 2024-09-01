using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using NArtadoSearch.Business.Services.Abstractions;
using NArtadoSearch.Core.DataAccess.ElasticSearch.Abstractions;
using NArtadoSearch.Core.Utilities.Mapping.Abstractions;
using NArtadoSearch.DataAccess.Abstractions.Read;
using NArtadoSearch.DataAccess.Abstractions.Write;
using NArtadoSearch.Entities.Concrete;
using NArtadoSearch.Entities.Dto;
using RabbitMQ.Client;

namespace NArtadoSearch.Business.Services;

public class WebIndexService(
    IIndexedWebUrlWriteRepository indexedWebUrlWriteRepository,
    IIndexedWebUrlReadRepository indexedWebUrlReadRepository,
    IQueryService<IndexedWebUrl> queryService,
    IEntityMapper<IndexWebUrlDto, IndexedWebUrl> mapper) : IWebIndexService
{
    public async Task AddAsync(IndexWebUrlDto data)
    {
        var existing = await indexedWebUrlReadRepository.GetSingle(x => x.Url == data.Url);
        if (existing != null)
        {
            await UpdateAsync(data);
            return;
        }

        var sameTitledEntity = await indexedWebUrlReadRepository.GetSingle(x => 
                x.ArticlesContent == data.ArticlesContent &&
                x.Title == data.Title &&
                x.Description == data.Description
            );
        if (sameTitledEntity != null)
        {
            return;
        }

        var entity = await mapper.MapAsync(data);
        entity.Popularity = 0;

        await indexedWebUrlWriteRepository.AddAsync(entity);

        await queryService.AddAsync(entity);
    }

    public async Task BulkAddAsync(IEnumerable<IndexWebUrlDto> data)
    {
        foreach (var dto in data)
        {
            await AddAsync(dto);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await indexedWebUrlReadRepository.GetSingle(x => x.Id == id);
        await indexedWebUrlWriteRepository.DeleteAsync(entity);
    }

    public async Task BulkDeleteAsync(IEnumerable<int> ids)
    {
        await indexedWebUrlWriteRepository.DeleteRangeAsync(ids);
    }

    public async Task UpdateAsync(IndexWebUrlDto data)
    {
        var entity = await indexedWebUrlReadRepository.GetSingle(x => x.Url == data.Url);
        var result = await mapper.MapAsync(data, entity);
        await indexedWebUrlWriteRepository.UpdateAsync(entity);

        await queryService.UpdateAsync(result);
    }

    public async Task SynchronizeAsync()
    {
        var allEntities = indexedWebUrlReadRepository.GetAll();
        await queryService.PurgeAsync();

        foreach (var entity in allEntities)
        {
            await queryService.AddAsync(entity);
        }

        Console.WriteLine("Database synchronized.");
    }
}