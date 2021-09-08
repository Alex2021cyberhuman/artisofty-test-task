using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Logic.Users.Models;

namespace Api.Authorization
{
    public class ClaimsFactory : IClaimsFactory
    {
        public Task<IEnumerable<Claim>> GetClaims(User user, CancellationToken cancellationToken = default) =>
            Task.FromResult(new Claim[]
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            }.AsEnumerable());
    }
}