using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.OpenApi
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!CheckApiController(context))
                return;

            var authorizeAttributes = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Union(context.MethodInfo.DeclaringType!
                    .GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>()
                );

            var allowAnonymousAttributes = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AllowAnonymousAttribute>();

            if (!allowAnonymousAttributes.Any() && authorizeAttributes.Any())
            {
                operation.Responses.Add("401", new()
                {
                    Description = "Unauthorized"
                });
                operation.Responses.Add("403", new()
                {
                    Description = "Forbidden"
                });

                var referenceScheme = new OpenApiSecurityScheme()
                {
                    Reference = new()
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    }
                };
                
                operation.Security.Add(new()
                {
                    [referenceScheme] = Array.Empty<string>()
                });
            }
        }

        private static bool CheckApiController(OperationFilterContext context) =>
            context.MethodInfo.DeclaringType != null &&
            context.MethodInfo.DeclaringType
                .GetCustomAttributes(true)
                .OfType<ApiControllerAttribute>()
                .Any();
    }
}