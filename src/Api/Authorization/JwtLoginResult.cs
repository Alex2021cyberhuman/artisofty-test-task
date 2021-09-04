namespace Api.Authorization
{
    public record JwtLoginResult
    {
        public JwtLoginResult(string accessToken)
        {
            AccessToken = accessToken;
        }

        public bool IsSuccessful => string.IsNullOrWhiteSpace(AccessToken);
        
        public string AccessToken { get; init; }
    }
}