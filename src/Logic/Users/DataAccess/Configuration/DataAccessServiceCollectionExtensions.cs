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
                throw new NotImplementedException("PostgreSqlUserRepository isn't implemented");
            }
            serviceCollection.AddSingleton<IUserRepository, MockUserRepository>();

            return serviceCollection;
        }
    }
}