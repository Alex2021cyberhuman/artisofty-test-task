using System.Threading;
using System.Threading.Tasks;
using Logic.Accounts.Models;
using Logic.Users.Models;

namespace Logic.Accounts.Interfaces
{
    public interface IAccountManager
    {
        Task<AccountResult> RegisterAsync(RegisterModel model, CancellationToken cancellationToken = default);
        Task<LoginResult> LoginAsync(LoginModel model, CancellationToken cancellationToken = default);
        Task<User?> GetUserInfoAsync(CancellationToken cancellationToken = default);
    }
}