using Logic.Accounts;
using Logic.Accounts.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AccountsServiceCollectionExtensions
    {
        public static IServiceCollection AddAccountsServices(
            this IServiceCollection services) =>
            services.AddScoped<IAccountManager, AccountManager>();
    }
}