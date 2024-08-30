using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NArtadoSearch.DataAccess.EntityFramework.Context;
using NArtadoSearch.DataAccess.EntityFramework.Context.Configuration;

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
    }
}