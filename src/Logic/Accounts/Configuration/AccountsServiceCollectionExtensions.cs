using Logic.Accounts;
using Logic.Accounts.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AccountsServiceCollectionExtensions
    {
        public static IServiceCollection AddAccountsServices<TLoginProcessor>(
            this IServiceCollection services) 
            where TLoginProcessor : class, ILoginProcessor =>
            services.AddScoped<AccountManager>()
                .AddScoped<ILoginProcessor, TLoginProcessor>();
    }
}