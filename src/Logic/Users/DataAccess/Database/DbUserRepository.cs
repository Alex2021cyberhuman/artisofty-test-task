using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Logic.Users.DataAccess.Database.Commands;
using Logic.Users.DataAccess.Database.Connection;
using Logic.Users.DataAccess.Interfaces;
using Logic.Users.Models;

namespace Logic.Users.DataAccess.Database
{
    public class DbUserRepository : IUserRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public DbUserRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User> CreateUserAsync(User user,
            CancellationToken cancellationToken = default)
        {
            await using var connection = _connectionFactory.CreateConnection();
            var command =
                DbUserCommonCommands.CreateInsertCommand(user,
                    cancellationToken);
            await connection.OpenAsync(cancellationToken);
            var createdUser = await connection.QueryFirstAsync<User>(command);
            return createdUser;
        }

        public async Task<bool> CheckUniquePhoneAsync(string phone,
            CancellationToken cancellationToken = default)
        {
            await using var connection = _connectionFactory.CreateConnection();
            var command =
                DbUserCommonCommands.CreateCheckUniquePhoneCommand(phone,
                    cancellationToken);
            var result =
                await connection.QueryFirstOrDefaultAsync<int?>(command);
            return result is null;
        }

        public async Task<bool> CheckUniqueEmailAsync(string email,
            CancellationToken cancellationToken = default)
        {
            await using var connection = _connectionFactory.CreateConnection();
            var command =
                DbUserCommonCommands.CreateCheckUniqueEmailCommand(email,
                    cancellationToken);
            var result =
                await connection.QueryFirstOrDefaultAsync<int?>(command);
            return result is null;
        }

        public async Task<User?> FindUserByPhonePasswordAsync(string phone,
            string password, CancellationToken cancellationToken = default)
        {
            await using var connection = _connectionFactory.CreateConnection();
            var command =
                DbUserCommonCommands.CreateFindUserByPhonePasswordCommand(phone,
                    password, cancellationToken);
            var user =
                await connection.QueryFirstOrDefaultAsync<User?>(command);
            return user;
        }

        public async Task UpdateLastLoginAsync(int userId, DateTime lastLogin,
            CancellationToken cancellationToken = default)
        {
            await using var connection = _connectionFactory.CreateConnection();
            var command =
                DbUserCommonCommands.CreateUpdateLastLoginCommand(userId,
                    lastLogin, cancellationToken);
            _ = await connection.ExecuteAsync(command);
        }

        public async Task<User?> FindUserByIdAsync(int id,
            CancellationToken cancellationToken = default)
        {
            await using var connection = _connectionFactory.CreateConnection();
            var command =
                DbUserCommonCommands.CreateFindUserByIdCommand(id,
                    cancellationToken);
            var user =
                await connection.QueryFirstOrDefaultAsync<User?>(command);
            return user;
        }
    }
}