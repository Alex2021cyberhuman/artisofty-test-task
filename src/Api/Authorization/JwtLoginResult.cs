using System.Text.Json.Serialization;
using Logic.Accounts.Models;

namespace Api.Authorization
{
    public record JwtLoginResult : LoginResult
    {
        public JwtLoginResult(string accessToken)
        {
            AccessToken = accessToken;
        }

        [JsonIgnore] public override bool IsSuccessful => base.IsSuccessful;

        [JsonIgnore]
        public override string Code
        {
            get => base.Code;
            init => base.Code = value;
        }

        [JsonIgnore]
        public override string Message
        {
            get => base.Message;
            init => base.Message = value;
        }

        public string AccessToken { get; init; }
    }
}