using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using NArtadoSearch.DataAccess.EntityFramework.Context.Configuration;

namespace NArtadoSearch.DataAccess.EntityFramework.Context;

public class MySqlDbContext(MySqlContextConfiguration configuration) : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var mysqlConnectionString = configuration.ConnectionString;
        MySqlConnection conn = new(mysqlConnectionString);
        optionsBuilder.UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(conn));
        base.OnConfiguring(optionsBuilder);
    }
}