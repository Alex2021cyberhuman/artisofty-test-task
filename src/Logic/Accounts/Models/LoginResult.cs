using Logic.Users.Models;

namespace Logic.Accounts.Models
{
    public abstract record LoginResult(User? User = null) : AccountResult;
}