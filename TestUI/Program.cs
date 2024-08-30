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
serviceCollection.AddElasticSearch(sp => new ElasticsearchClientSettings("", new ApiKey("http://localhost:9200")));
var serviceProvider = serviceCollection.BuildServiceProvider();

var elasticClient = serviceProvider.GetRequiredService<ElasticsearchClient>();
