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
            {
                user = GetUserWithNextId(user);
            }

            return _dictionary.TryAdd(user.Id, user)
                ? Task.FromResult(user)
                : throw new InvalidOperationException("Cannot write to user dictionary");
        }

        public Task<bool> CheckUniquePhoneAsync(string phone, CancellationToken cancellationToken = default) =>
            Task.FromResult(_dictionary.Values.All(user => user.Phone != phone));

        public Task<bool> CheckUniqueEmailAsync(string email, CancellationToken cancellationToken = default) =>
            Task.FromResult(_dictionary.Values.All(user => user.Email != email));

        public Task<User?> TryLoginAsync(string phone, string password,
            CancellationToken cancellationToken = default)
        {
            var user = _dictionary.Values.FirstOrDefault(user => user.Phone == phone && user.Password == password);
            if (user is null)
                return Task.FromResult<User?>(null);
            user = UserMutationHelper.GetUserWithUpdatedLastLogin(DateTime.UtcNow, user);
            _dictionary[user.Id] = user;
            return Task.FromResult<User?>(user);
        }
        
        public Task<User?> FindUserByIdAsync(int id, CancellationToken cancellationToken) =>
            Task.FromResult(_dictionary.GetValueOrDefault(id));

        private User GetUserWithNextId(User user) =>
            user with
            {
                Id = _dictionary.Keys.Max() + 1
            };
    }
}