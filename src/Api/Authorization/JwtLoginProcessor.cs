using System;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Logic.Accounts.Interfaces;
using Logic.Accounts.Models;
using Logic.Users.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api.Authorization
{
    public class JwtLoginProcessor : ILoginProcessor
    {
        private readonly IClaimsFactory _claimsFactory;
        private readonly JwtWriteOptions _options;

        public JwtLoginProcessor(IClaimsFactory claimsFactory, IOptions<JwtWriteOptions> options)
        {
            _claimsFactory = claimsFactory;
            _options = options.Value;
        }

        public async Task<LoginResult> ProcessLoginAsync(User user, CancellationToken cancellationToken = default)
        {
            var claims = await _claimsFactory.GetClaims(user, cancellationToken);
            var claimsIdentity = new ClaimsIdentity(
                claims, JwtBearerDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

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
            return new JwtLoginResult(accessToken)
            {
                User = user
            };
        }
    }
}