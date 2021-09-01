using System.Threading;
using System.Threading.Tasks;
using Logic.Accounts.Models;

namespace Logic.Accounts.Interfaces
{
    public interface IAccountManager
    {
        Task<AccountResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
        Task<LoginResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    }
}