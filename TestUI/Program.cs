using System.Diagnostics;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.DependencyInjection;
using NArtadoSearch.Core.Extensions;
using NArtadoSearch.Core.Security.Authentication.Abstractions;
using NArtadoSearch.Core.Security.Authentication.Artado;
using NArtadoSearch.Core.Utilities.Caching.Abstractions;
using Newtonsoft.Json;

var serviceCollection = new ServiceCollection();
serviceCollection.AddElasticSearch(sp => new ElasticsearchClientSettings(
    "96809f23073e462c8a370e1555c838de:dXMtY2VudHJhbDEuZ2NwLmNsb3VkLmVzLmlvJGZhMzdkN2ZhMjVhOTRlY2NhZTZhODBkNTA3MzhmNmIzJDczNDc0OTkzM2JjZjQ4ZGM5MzVlZmVjYzkxOGU1YzE3",
    new ApiKey("TWFMd29wRUI5RlhVTktIcG5KRng6UVpIX0lQUC1ROE9aSHd6aW9TZjgwQQ==")));
var serviceProvider = serviceCollection.BuildServiceProvider();

var elasticClient = serviceProvider.GetRequiredService<ElasticsearchClient>();
var getIndexResponse = await elasticClient.Indices.GetAsync("search-artado");
var searchInSite =
    await elasticClient.SearchAsync<Entity>(s =>
        s.Index("search-artado").Query(q => q.QueryString(d=>d.Query("pottie4r"))).Size(1000));

if (!searchInSite.IsValidResponse)
{
    Console.WriteLine("Invalid search response");
}

if (searchInSite.IsValidResponse)
{
    Console.WriteLine(JsonConvert.SerializeObject(searchInSite.Documents, Formatting.Indented));
}

Console.ReadKey();


class Entity
{
    public string Title { get; set; }
    public string Url { get; set; }
}