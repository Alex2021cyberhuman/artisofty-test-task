using System;
using Logic.Users.DataAccess.Configuration;
using Logic.Users.DataAccess.Interfaces;
using Logic.Users.DataAccess.Mock;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class DataAccessServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, Action<DataAccessOptions>? optionsAction)
        {
            optionsAction ??= _ => { };
            var dataAccessOptions = DataAccessOptions.Default;
            services.Configure<DataAccessOptions>(options => optionsAction(options));
            optionsAction(dataAccessOptions);

            
            if (dataAccessOptions.UseDatabase)
            {
                services.AddDatabaseDataAccessServices(dataAccessOptions);
            }
            else
            {
                services.AddSingleton<IUserRepository, MockUserRepository>();
            }

            return services;
        }
    }
}