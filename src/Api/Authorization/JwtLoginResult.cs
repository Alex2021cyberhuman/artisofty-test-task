using System;

namespace Api.Authorization
{
    public record JwtLoginResult
    {
        public JwtLoginResult(string accessToken, DateTime expires)
        {
            AccessToken = accessToken;
            Expires = expires;
        }

        public bool IsSuccessful => string.IsNullOrWhiteSpace(AccessToken);
        
        public string AccessToken { get; init; }

        public DateTime Expires { get; init; }
    }
}