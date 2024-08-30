using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NArtadoSearch.Business.Mappers;
using NArtadoSearch.Business.Services;
using NArtadoSearch.Business.Services.Abstractions;
using NArtadoSearch.Business.Services.Hosted;
using NArtadoSearch.Core.DataAccess.ElasticSearch;
using NArtadoSearch.Core.DataAccess.ElasticSearch.Abstractions;
using NArtadoSearch.Core.Utilities.Mapping.Abstractions;
using NArtadoSearch.DataAccess.Abstractions.Read;
using NArtadoSearch.DataAccess.Abstractions.Write;
using NArtadoSearch.DataAccess.EntityFramework.Context;
using NArtadoSearch.DataAccess.EntityFramework.Context.Configuration;
using NArtadoSearch.DataAccess.EntityFramework.Read;
using NArtadoSearch.DataAccess.EntityFramework.Write;
using NArtadoSearch.Entities.Concrete;
using NArtadoSearch.Entities.Dto;
using NArtadoSearch.Entities.Models;

namespace NArtadoSearch.Business.Registration;

public static class ServiceCollectionExtensions
{
    public static void AddArtadoServices(this IServiceCollection services, MySqlContextConfiguration mysqlContextConfiguration)
    {
        services.AddSingleton<MySqlContextConfiguration>(mysqlContextConfiguration);
        services.AddScoped<DbContext, MySqlDbContext>();
        services.AddMediatR(c =>
        {
            c.Lifetime = ServiceLifetime.Scoped;
            c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddScoped<IWebIndexService, WebIndexService>();
        services.AddScoped<IIndexedWebUrlReadRepository, EfIndexedWebUrlReadRepository>();
        services.AddScoped<IIndexedWebUrlWriteRepository, EfIndexedWebUrlWriteRepository>();

        services.AddScoped<IEntityMapper<IndexWebUrlDto, IndexedWebUrl>, WebIndexMapper>();
        services.AddSingleton<IQueryService<IndexedWebUrl>, ElasticQueryService<IndexedWebUrl>>();
        services.AddHostedService<BackgroundIndexerService>();
    }
}