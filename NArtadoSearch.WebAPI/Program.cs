using NArtadoSearch.Core.Extensions;

namespace NArtadoSearch.WebAPI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddAuthorization();
        await builder.Services.AddRedisCache(c =>
        {
            c.AllowAdmin = true;
            c.EndPoints.Add(builder.Configuration["Redis:Endpoint"]!);
        });
        
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

        await app.RunAsync();
    }
}