using System.Threading;
using System.Threading.Tasks;
using Logic.Users.Models;

namespace Api.Authorization.Interfaces
{
    public interface IJwtLoginProcessor
    {
        Task<JwtLoginResult> ProcessLoginAsync(User user, CancellationToken cancellationToken = default);
    }
}