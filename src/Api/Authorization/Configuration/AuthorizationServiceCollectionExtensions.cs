using Logic.Accounts.Configuration;
using Logic.Accounts.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Authorization.Configuration
{
    public static class AuthorizationServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomAuthorization(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            var jwtWriteOptions = new JwtWriteOptions();
            configuration.Bind("JwtWriteOptions", jwtWriteOptions);

            return services.Configure<JwtWriteOptions>(options =>
                    configuration.Bind("JwtWriteOptions", options))
                .AddHttpContextAccessor()
                .AddScoped<IAuthenticatedUserIdentifierProvider, ClaimIdentifierProvider>()
                .AddScoped<IClaimsFactory, ClaimsFactory>()
                .AddAccountsServices<JwtLoginProcessor>()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    configuration.Bind("JwtBearerOptions", options);
                    jwtWriteOptions.ApplyToBearerOptions(options);
                })
                .Services
                .AddAuthorization(options => options.DefaultPolicy = new 
                        AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());
        }
    }
}