using Api.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
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