using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Api.Areas.Api.Authorization.Interfaces;
using Api.Authorization.Interfaces;
using Logic.Users.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api.Areas.Api.Authorization
{
    public class JwtLoginProcessor : IJwtLoginProcessor
    {
        private readonly IClaimsFactory _claimsFactory;
        private readonly JwtWriteOptions _options;

        public JwtLoginProcessor(IClaimsFactory claimsFactory,
            IOptions<JwtWriteOptions> options)
        {
            _claimsFactory = claimsFactory;
            _options = options.Value;
        }

        public async Task<JwtLoginResult> ProcessLoginAsync(User user,
            CancellationToken cancellationToken = default)
        {
            var claims =
                await _claimsFactory.GetClaims(user, cancellationToken);
            var claimsIdentity = new ClaimsIdentity(
                claims, JwtBearerDefaults.AuthenticationScheme, ClaimTypes.Name,
                ClaimTypes.Role);

            var notBefore = DateTime.UtcNow;
            var expires = notBefore.Add(_options.TokenLifetime);
            var signingCredentials = new SigningCredentials(
                _options.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claimsIdentity.Claims,
                notBefore,
                expires,
                signingCredentials);
            var securityTokenHandler = new JwtSecurityTokenHandler();
            var accessToken = securityTokenHandler.WriteToken(securityToken);
            return new(accessToken, expires);
        }
    }
}