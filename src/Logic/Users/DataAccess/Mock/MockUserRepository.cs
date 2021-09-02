using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Logic.Users.DataAccess.Interfaces;
using Logic.Users.Models;
using Logic.Users.Utilities;

namespace Logic.Users.DataAccess.Mock
{
    public class MockUserRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<int, User> _dictionary = new();

        public Task<User> CreateUserAsync(User user, CancellationToken cancellationToken = default)
        {
            if (user.Id == default)
                user = GetUserWithNextId(user);

            return _dictionary.TryAdd(user.Id, user)
                ? Task.FromResult(user)
                : throw new InvalidOperationException("Cannot write to user dictionary");
        }

        public Task<bool> CheckUniquePhoneAsync(string phone, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_dictionary.Values.All(user => user.Phone != phone));
        }

        public Task<bool> CheckUniqueEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_dictionary.Values.All(user => user.Email != email));
        }

        public Task<User?> FindUserByPhonePasswordAsync(string phone, string password,
            CancellationToken cancellationToken = default)
        {
            var foundUser = _dictionary.Values.FirstOrDefault(user => user.Phone == phone && user.Password == password);
            return foundUser is null
                ? Task.FromResult<User?>(null)
                : Task.FromResult<User?>(foundUser);
        }

        public Task UpdateLastLoginAsync(int userId, DateTime lastLogin, CancellationToken cancellationToken = default)
        {
            var user = _dictionary.GetValueOrDefault(userId);
            if (user is null)
                return Task.CompletedTask;
            user = UserMutationHelper.GetUserWithUpdatedLastLogin(DateTime.UtcNow, user);
            _dictionary[user.Id] = user;
            return Task.CompletedTask;
        }

        public Task<User?> FindUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_dictionary.GetValueOrDefault(id));
        }

        private User GetUserWithNextId(User user)
        {
            return user with
            {
                Id = _dictionary.Any() ? _dictionary.Keys.Max() + 1 : 1
            };
        }
    }
}