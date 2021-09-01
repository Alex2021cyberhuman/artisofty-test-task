using System;
using Microsoft.EntityFrameworkCore;

namespace Logic.Users.DataAccess.Database.DbContexts
{
    public static class DbContextOptionsExtensions
    {
        private const string MigrationTableName = "_EFCoreUsersDatabaseMigrationTable";

        public static TBuilder ConfigureDebugging<TBuilder>(this TBuilder builder, DatabaseOptions options)
            where TBuilder : DbContextOptionsBuilder
        {
            if (options.EnableDebug)
                builder.EnableDetailedErrors().EnableSensitiveDataLogging();
            return builder;
        }

        public static TBuilder ConfigureProviderType<TBuilder>(this TBuilder builder, DatabaseOptions options)
            where TBuilder : DbContextOptionsBuilder
        {
            ConfigureProvider(builder, options.ProviderType, options.ConnectionString);
            return builder;
        }

        private static void ConfigureProvider<TBuilder>(this TBuilder builder,
            ProviderType providerType,
            string connectionString)
            where TBuilder : DbContextOptionsBuilder
        {
            switch (providerType)
            {
                case ProviderType.MsSql:
                    builder.UseSqlServer(connectionString,
                        sqlOptions => sqlOptions
                            .MigrationsAssembly("Logic.DataAccess.Migrations.MsSql")
                            .MigrationsHistoryTable(MigrationTableName));
                    break;
                case ProviderType.PostgreSql:
                    builder.UseNpgsql(connectionString,
                        sqlOptions => sqlOptions
                            .MigrationsAssembly("Logic.DataAccess.Migrations.PostgreSql")
                            .MigrationsHistoryTable(MigrationTableName));
                    break;
                case ProviderType.MySql:
                    builder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0)),
                        sqlOptions => sqlOptions
                            .MigrationsAssembly("Logic.DataAccess.Migrations.MySql")
                            .MigrationsHistoryTable(MigrationTableName));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(providerType));
            }
        }
    }
}