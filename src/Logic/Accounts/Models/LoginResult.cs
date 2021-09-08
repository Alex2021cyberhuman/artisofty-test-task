using Logic.Users.Models;

namespace Logic.Accounts.Models
{
    public record LoginResult(User? User = null) : AccountResult
    {
        public override bool IsSuccessful => base.IsSuccessful && User is not null;
    }
}