using System.Security.Claims;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.AspNetCore.RateLimiting;
using NArtadoSearch.Business.Registration;
using NArtadoSearch.Core.Extensions;
using NArtadoSearch.Core.Security.Authentication.Artado;
using NArtadoSearch.DataAccess.EntityFramework.Context.Configuration;
using RabbitMQ.Client;

namespace NArtadoSearch.WebAPI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddAuthorization();

        builder.Services.AddRateLimiter(options => { });

        builder.Services.AddMicrosoftMemoryCache();
        builder.Services.AddElasticSearch(c =>
            new ElasticsearchClientSettings(builder.Configuration["ElasticSearch:CloudId"]!,
                new ApiKey(builder.Configuration["ElasticSearch:ApiKey"]!)));

        builder.Services.AddRabbitMqEventBus(factory => { factory.Endpoint = new AmqpTcpEndpoint("localhost"); });

        builder.Services.AddArtadoServices(builder.Configuration.GetSection("MySqlConfiguration")
            .Get<MySqlContextConfiguration>()!);
        builder.Services.Configure<ArtadoTokenOptions>(builder.Configuration.GetSection("ArtadoTokenOptions"));

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseCors(policy =>
        {
            policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        });
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRateLimiter(new RateLimiterOptions()
        {
            RejectionStatusCode = StatusCodes.Status503ServiceUnavailable,
            OnRejected = (async (context, token) =>
            {
                await context.HttpContext.Response.WriteAsync(
                    "Service temporarily unavailable. Please try again later.", token);
            })
        });

        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}