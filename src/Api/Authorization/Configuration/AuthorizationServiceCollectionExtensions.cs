using System.Threading.Tasks;
using Api.Authorization.Interfaces;
using Logic.Accounts.Configuration;
using Logic.Accounts.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
                .AddScoped<IJwtLoginProcessor, JwtLoginProcessor>()
                .AddAccountsServices()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddCookie()
                .AddJwtBearer(options =>
                {
                    options.Events = new()
                    {
                        OnMessageReceived = context => OnMessageReceived(context, jwtWriteOptions)
                    };
                    configuration.Bind("JwtBearerOptions", options);
                    jwtWriteOptions.ApplyToBearerOptions(options);
                })
                .Services
                .AddCookiePolicy(options =>
                {
                    options.CheckConsentNeeded = _ => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                })
                .AddAuthorization(options => options.DefaultPolicy = new
                        AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());
        }

        private static Task OnMessageReceived(MessageReceivedContext context, JwtWriteOptions options)
        {
            var request = context.HttpContext.Request;
            var cookies = request.Cookies;
            if (cookies.TryGetValue(options.AccessTokenCookieName, out var accessTokenValue))
            {
                context.Token = accessTokenValue;
                return Task.CompletedTask;
            }
            context.Fail("cannot get access token cookie");
            return Task.CompletedTask;
        }
    }
}