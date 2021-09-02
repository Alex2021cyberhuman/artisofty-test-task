using Logic.Accounts.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Logic.Accounts.Configuration
{
    public static class AccountsServiceCollectionExtensions
    {
        public static IServiceCollection AddAccountsServices<TLoginProcessor>(
            this IServiceCollection services) 
            where TLoginProcessor : class, ILoginProcessor =>
            services.AddScoped<IAccountManager, AccountManager>()
                .AddScoped<ILoginProcessor, TLoginProcessor>();
    }
}