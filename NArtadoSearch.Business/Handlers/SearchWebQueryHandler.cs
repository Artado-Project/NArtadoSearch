using System.Diagnostics;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Nodes;
using MediatR;
using Microsoft.Extensions.Logging;
using NArtadoSearch.Business.Queries;
using NArtadoSearch.Core.DataAccess.ElasticSearch.Abstractions;
using NArtadoSearch.Entities.Concrete;
using NArtadoSearch.Entities.Models;

namespace NArtadoSearch.Business.Handlers;

public class SearchWebQueryHandler(ElasticsearchClient client, ILogger<SearchWebQueryHandler> logger)
    : IRequestHandler<SearchWebQuery, WebSearchResults>
{
    private readonly ElasticsearchClient _client = client;
    private readonly ILogger<SearchWebQueryHandler> _logger = logger;

    public async Task<WebSearchResults> Handle(SearchWebQuery request, CancellationToken cancellationToken)
    {
        Stopwatch sw = Stopwatch.StartNew();
        var response = await _client.SearchAsync<WebSearchResultItem>(
            s => s.Index("search-artado").Query(t => t.QueryString(r => r.Query(request.SearchText))).Size(20),
            cancellationToken);
        sw.Stop();
        if (response.IsValidResponse)
        {
            return new WebSearchResults
            {
                Results = response.Documents,
                ElapsedMilliseconds = sw.ElapsedMilliseconds,
                Page = 1,
                Size = response.Documents.Count
            };
        }

        _logger.LogInformation("Search query failed.");
        return new WebSearchResults();
    }
}