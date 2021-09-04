using Logic.Users.Models;

namespace Logic.Accounts.Models
{
    public record LoginResult(User? User = null) : AccountResult;
}