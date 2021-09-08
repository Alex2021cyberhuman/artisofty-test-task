using Logic.Accounts;
using Logic.Accounts.Interfaces;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class AccountsServiceCollectionExtensions
    {
        public static IServiceCollection AddAccountsServices(
            this IServiceCollection services)
        {
            return services.AddScoped<IAccountManager, AccountManager>();
        }
    }
}