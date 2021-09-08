using Dapper.FluentMap;
using FluentMigrator.Runner;
using Logic.Users.DataAccess.Configuration;
using Logic.Users.DataAccess.Database;
using Logic.Users.DataAccess.Database.Connection;
using Logic.Users.DataAccess.Database.FluentMigrations;
using Logic.Users.DataAccess.Database.Maps;
using Logic.Users.DataAccess.Interfaces;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class DatabaseDataAccessServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseDataAccessServices(
            this IServiceCollection services, DataAccessOptions options)
        {
            FluentMapper.Initialize(configuration =>
            {
                configuration.AddMap(new UserMap());
            });
            return services
                .AddSingleton<IConnectionFactory, ConnectionFactory>()
                .AddSingleton<IUserRepository, DbUserRepository>()
                .AddFluentMigratorCore()
                .ConfigureRunner(builder => builder
                    .AddPostgres()
                    .WithGlobalConnectionString(serviceProvider =>
                        serviceProvider
                            .GetRequiredService<IOptions<DataAccessOptions>>()
                            .Value.DatabaseOptions.ConnectionString)
                    .ScanIn(typeof(InitialFluentMigration).Assembly));
        }
    }
}