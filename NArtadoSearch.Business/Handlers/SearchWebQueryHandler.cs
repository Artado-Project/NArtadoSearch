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

public class SearchWebQueryHandler(
    IQueryService<IndexedWebUrl> queryService,
    ILogger<SearchWebQueryHandler> logger)
    : IRequestHandler<SearchWebQuery, WebSearchResults>
{
    private readonly ILogger<SearchWebQueryHandler> _logger = logger;

    public async Task<WebSearchResults> Handle(SearchWebQuery request, CancellationToken cancellationToken)
    {
        Stopwatch sw = Stopwatch.StartNew();
        var response = await queryService.SearchAsync(request.SearchText);
        sw.Stop();
        
        return new WebSearchResults
        {
            Results = response.Select(x=> new WebSearchResultItem(){ Title = x.Title, Url = x.Url }),
            ElapsedMilliseconds = sw.ElapsedMilliseconds,
            Page = 1,
            Size = response.Count()
        };
    }
}