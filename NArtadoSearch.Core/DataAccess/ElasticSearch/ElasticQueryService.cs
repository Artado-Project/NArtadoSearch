using System.Globalization;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using NArtadoSearch.Core.DataAccess.ElasticSearch.Abstractions;
using NArtadoSearch.Core.Entities;

namespace NArtadoSearch.Core.DataAccess.ElasticSearch;

public class ElasticQueryService<T>(ElasticsearchClient client) : IQueryService<T>
    where T : class, IEntity, new()
{
    private readonly ElasticsearchClient _client = client;

    private string _indexName = $"artado-search-{CultureInfo.GetCultureInfo("en-US").TextInfo.ToLower(typeof(T).Name)}";

    public Task<T> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<T>> SearchAsync(string query, int page = 1, int pageSize = 20)
    {
        var index = await _client.Indices.GetAsync<T>(_indexName);
        if (!index.IsValidResponse)
        {
            await _client.Indices.CreateAsync<T>(_indexName);
        }

        var response = await _client.SearchAsync<T>(q =>
            q.Index(_indexName).Query(t => t.MultiMatch(r =>
                    r.Query(query).Type(TextQueryType.MostFields).Fuzziness(new Fuzziness("AUTO"))
                        .Fields(Fields.FromString("*"))
                        .Analyzer("custom_edge_ngram_analyzer")))
                .From((page - 1) * pageSize)
                .Size(pageSize));
        
        if (response.IsSuccess())
            return response.Documents;
        return new List<T>();
    }

    public async Task PurgeAsync()
    {
        await _client.Indices.FlushAsync<T>(_indexName);
    }

    public async Task AddAsync(T entity)
    {
        var index = await _client.Indices.GetAsync<T>(_indexName);
        if (!index.IsValidResponse)
        {
            await _client.Indices.CreateAsync<T>(_indexName);
        }

        await _client.IndexAsync(entity, i => i.Index(_indexName));
    }

    public async Task UpdateAsync(T entity)
    {
        var index = await _client.Indices.GetAsync<T>(_indexName);
        if (!index.IsValidResponse)
        {
            return;
        }

        await _client.UpdateAsync<T, T>(entity.Id, i => i.Index(_indexName).Doc(entity));
    }
}