using System.Threading;
using System.Threading.Tasks;

namespace Logic.Accounts.Interfaces
{
    public interface IAuthenticatedUserIdentifierProvider
    {
        Task<int> GetUserIdAsync(CancellationToken cancellationToken);
    }
}