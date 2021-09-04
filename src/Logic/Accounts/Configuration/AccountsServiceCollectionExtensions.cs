using Logic.Accounts.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Logic.Accounts.Configuration
{
    public static class AccountsServiceCollectionExtensions
    {
        public static IServiceCollection AddAccountsServices(
            this IServiceCollection services) =>
            services.AddScoped<IAccountManager, AccountManager>();
    }
}