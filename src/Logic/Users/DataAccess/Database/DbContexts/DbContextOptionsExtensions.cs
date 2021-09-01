using System;
using Logic.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Logic.Users.DataAccess.Database
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
            var connectionString = options.ConnectionString;
            switch (options.ProviderType)
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
                            .MigrationsAssembly("Logic.DataAccess.Migrations.MsSql")
                            .MigrationsHistoryTable(MigrationTableName));
                    break;
                case ProviderType.MySql:
                    builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                        sqlOptions => sqlOptions
                            .MigrationsAssembly("Logic.DataAccess.Migrations.MsSql")
                            .MigrationsHistoryTable(MigrationTableName));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return builder;
        }
    }
}