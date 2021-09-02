using System.Threading.Tasks;
using Dapper;
using FluentMigrator.Runner;
using Logic.Users.DataAccess.Configuration;
using Logic.Users.DataAccess.Database.Commands;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
            var connection = factory.CreateConnection();
            await connection.ExecuteAsync(DbInitialCommands.GetCreateDatabaseCommand("test_backend_db"));
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }
}