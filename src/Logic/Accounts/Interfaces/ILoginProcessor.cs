using System.Threading;
using System.Threading.Tasks;
using Logic.Accounts.Models;
using Logic.Users.Models;

namespace Logic.Accounts.Interfaces
{
    public interface ILoginProcessor
    {
        /// <summary>
        /// Authorizes the user in the system.
        /// Form of authorization depends on type of platform and realization.
        /// For example, in web api will give jwt, and in web app it will give cookies
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<LoginResult> ProcessLoginAsync(User user, CancellationToken cancellationToken = default);
    }
}