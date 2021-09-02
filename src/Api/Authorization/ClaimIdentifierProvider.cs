using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Logic.Accounts.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Api.Authorization
{
    public class ClaimIdentifierProvider : IAuthenticatedUserIdentifierProvider
    {
        private readonly IHttpContextAccessor _accessor;

        public ClaimIdentifierProvider(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Task<int> GetUserIdAsync(CancellationToken cancellationToken)
        {
            var user = _accessor.HttpContext!.User;
            var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);
            return Task.FromResult(int.Parse(userIdClaim));
        }
    }
}