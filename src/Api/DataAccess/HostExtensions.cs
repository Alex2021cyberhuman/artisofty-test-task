using System.Threading.Tasks;
using FluentMigrator.Runner;
using Logic.Users.DataAccess.Configuration;
using Logic.Users.DataAccess.Database.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
using IConnectionFactory = Logic.Users.DataAccess.Database.Connection.IConnectionFactory;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Hosting
{
    public static class HostExtensions
    {
        public static async Task UpdateDatabaseAsync(this IHost host)
        {
            var dataAccessOptions = host.Services.GetRequiredService<IOptions<DataAccessOptions>>();
            if (!dataAccessOptions.Value.UseDatabase)
                return;
            using var scope = host.Services.CreateScope();
            var factory = scope.ServiceProvider.GetRequiredService<IConnectionFactory>();
            await using var connection = factory.CreateConnection();
            await using var rootConnection = new NpgsqlConnection(
                new NpgsqlConnectionStringBuilder(connection.ConnectionString)
            {
                Database = "postgres"
            }.ConnectionString);
            await rootConnection.OpenAsync();
            await rootConnection.CreateDatabaseAsync(connection.Database);
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }
}