namespace Api.Areas.Api.Models
{
    public class TokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;

        public long Expires { get; set; }
    }
}