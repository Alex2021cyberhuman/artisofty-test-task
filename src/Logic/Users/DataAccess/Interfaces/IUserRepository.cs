using System.Threading;
using System.Threading.Tasks;
using Logic.Users.Models;

namespace Logic.Users.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Creates user with specified id or with next id if current is 0.
        /// This method does not validate <paramref name="user"/>.
        /// Make sure user has been validated on previous layers.
        /// </summary>
        /// <param name="user">User model to create</param>
        /// <param name="cancellationToken">Asynchronous task cancellation</param>
        /// <returns>Task with created user as result</returns>
        Task<User> CreateUserAsync(User user, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if specified <paramref name="phone"/> is unique,
        /// if phone is unique then returns true,
        /// else returns false 
        /// </summary>
        /// <param name="phone">User phone <see cref="User.Phone"/>.</param>
        /// <param name="cancellationToken">Asynchronous task cancellation</param>
        /// <returns>Task with true or false as result</returns>
        Task<bool> CheckUniquePhoneAsync(string phone, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if specified <paramref name="email"/> is unique,
        /// if email is unique then returns true,
        /// else returns false 
        /// </summary>
        /// <param name="email">User email <see cref="User.Email"/>.</param>
        /// <param name="cancellationToken">Asynchronous task cancellation</param>
        /// <returns>Task with true or false as result</returns>
        Task<bool> CheckUniqueEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tries to find user with these <paramref name="phone"/> and <paramref name="password"/>,
        /// if success then updates <see cref="User.LastLogin"/> of user and returns he,
        /// else returns <c>null</c> 
        /// </summary>
        /// <param name="phone">Formatted phone like 7(\d){10} <see cref="User.Phone"/>.
        /// Make sure phone format is right.
        /// This method does not validate input.</param>
        /// <param name="password">User password <see cref="User.Password"/></param>
        /// <param name="cancellationToken">Asynchronous task cancellation</param>
        /// <returns>Task with founded user or null as result</returns>
        Task<User?> TryLoginAsync(string phone, string password, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tries to find user with this id,
        /// if success then returns user,
        /// else returns <c>null</c>  
        /// </summary>
        /// <param name="id"><see cref="User.Id"/></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Task with founded user or null as result</returns>
        Task<User?> FindUserByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}