using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Logic.Users.Models;

namespace Api.Authorization.Interfaces
{
    public interface IClaimsFactory
    {
        /// <summary>
        /// Generates claims for specified user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<Claim>> GetClaims(User user,
            CancellationToken cancellationToken = default);
    }
}