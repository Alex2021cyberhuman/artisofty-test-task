using System;
using Logic.Users.DataAccess.Database;
using Logic.Users.DataAccess.Database.DbContexts;
using Logic.Users.DataAccess.Interfaces;
using Logic.Users.DataAccess.Mock;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class DataAccessServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection serviceCollection, Action<DataAccessOptions>? optionsAction)
        {
            optionsAction ??= _ => { };
            var dataAccessOptions = DataAccessOptions.Default;
            optionsAction(dataAccessOptions);

            if (dataAccessOptions.UseDatabase)
            {
                var databaseOptions = dataAccessOptions.DatabaseOptions;
                serviceCollection
                    .AddDbContext<UsersDbContext>(builder => builder
                        .ConfigureProviderType(databaseOptions)
                        .ConfigureDebugging(databaseOptions));
                serviceCollection.AddScoped<IUserRepository, EfUserRepository>();
            }
            else
            {
                serviceCollection.AddSingleton<IUserRepository, MockUserRepository>();
            }

            return serviceCollection;
        }
    }
}