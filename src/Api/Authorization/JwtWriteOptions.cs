using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Api.Authorization
{
    public class JwtWriteOptions
    {
        public string Issuer { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        public string SecurityKey { get; set; } = string.Empty;

        public TimeSpan TokenLifetime { get; set; }

        public SymmetricSecurityKey SymmetricSecurityKey => new(Encoding.ASCII.GetBytes(SecurityKey));

        public void ApplyToBearerOptions(JwtBearerOptions options)
        {
            var validationParameters = options.TokenValidationParameters;
            validationParameters.ValidIssuer = Issuer;
            validationParameters.ValidAudience = Audience;
            validationParameters.IssuerSigningKey = SymmetricSecurityKey;
            validationParameters.ValidateIssuerSigningKey = true;
        }
    }
}