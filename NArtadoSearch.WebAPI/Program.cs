using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using NArtadoSearch.Business.Registration;
using NArtadoSearch.Core.Extensions;
using NArtadoSearch.DataAccess.EntityFramework.Context.Configuration;

namespace NArtadoSearch.WebAPI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddAuthorization();
        builder.Services.AddMicrosoftMemoryCache();
        builder.Services.AddElasticSearch(c =>
            new ElasticsearchClientSettings(builder.Configuration["ElasticSearch:CloudId"]!,
                new ApiKey(builder.Configuration["ElasticSearch:ApiKey"]!)));
        builder.Services.AddArtadoServices(builder.Configuration.GetSection("MySqlConfiguration").Get<MySqlContextConfiguration>()!);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(); 
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}