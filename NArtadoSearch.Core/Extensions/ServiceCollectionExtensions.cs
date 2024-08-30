using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using NArtadoSearch.Core.DataAccess.ElasticSearch.Abstractions;
using NArtadoSearch.Core.Utilities.Caching.Abstractions;
using NArtadoSearch.Core.Utilities.Caching.Microsoft;
using NArtadoSearch.Core.Utilities.Caching.Redis;
using NArtadoSearch.Core.Utilities.EventBus.Abstractions;
using NArtadoSearch.Core.Utilities.EventBus.RabbitMQ;
using NArtadoSearch.Core.Utilities.Mapping.Abstractions;
using RabbitMQ.Client;
using StackExchange.Redis;

namespace NArtadoSearch.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddMicrosoftMemoryCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<ICacheManager, MicrosoftCacheManager>();
    }

    public static async Task AddRedisCache(this IServiceCollection services,
        Action<ConfigurationOptions>? configureOptions = null)
    {
        var configurationOptions = new ConfigurationOptions();
        if (configureOptions is not null)
            configureOptions?.Invoke(configurationOptions);

        try
        {
            var connection = await ConnectionMultiplexer.ConnectAsync(configurationOptions);
            services.AddSingleton<IConnectionMultiplexer>(connection);
            services.AddSingleton<ICacheManager, RedisCacheManager>();
        }
        catch (Exception ex)
        {
            throw new Exception(
                "Application Terminated: Unable to connect to Redis. Please check your configuration to connect redis.",
                ex);
        }
    }

    public static void AddElasticSearch(this IServiceCollection services,
        Func<IServiceProvider, ElasticsearchClientSettings> clientSettingsFactory)
    {
        services.AddSingleton(clientSettingsFactory);
        services.AddSingleton(provider =>
        {
            var client = new ElasticsearchClient(provider.GetRequiredService<ElasticsearchClientSettings>());
            return client;
        });
    }

    public static void AddRabbitMqEventBus(this IServiceCollection services,
        Action<ConnectionFactory> configureConnectionFactory)
    {
        services.AddSingleton<IConnection>(c =>
        {
            ConnectionFactory factory = new ConnectionFactory();
            configureConnectionFactory(factory);
            return factory.CreateConnection();
        });

        services.AddSingleton<IEventBus, RabbitMqEventBus>();
    }
}