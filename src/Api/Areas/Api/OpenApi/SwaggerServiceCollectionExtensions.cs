using System;
using System.IO;
using System.Reflection;
using Api.Areas.Api.OpenApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomSwagger(
            this IServiceCollection services)
        {
            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new()
                {
                    Title = "TestBack", Version = "v1"
                });
                options.IncludeXmlComments(
                    Path.Combine(
                        AppContext.BaseDirectory,
                        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
                options.AddSecurityDefinition(
                    JwtBearerDefaults.AuthenticationScheme,
                    new()
                    {
                        In = ParameterLocation.Header,
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Description = "Please insert JWT with Bearer into field"
                    });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }
    }
}