using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Api.Areas.Api.Authorization
{
    public class JwtWriteOptions
    {
        private TimeSpan _tokenLifetime;

        public string Issuer { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        public string SecurityKey { get; set; } = string.Empty;

        public TimeSpan TokenLifetime
        {
            get => _tokenLifetime;
            set
            {
                CookieOptions.MaxAge = value;
                _tokenLifetime = value;
            }
        }

        public string AccessTokenCookieName { get; set; } =
            "TestBack.AccessToken";

        public SymmetricSecurityKey SymmetricSecurityKey =>
            new(Encoding.ASCII.GetBytes(SecurityKey));

        public CookieOptions CookieOptions { get; set; } = new();

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