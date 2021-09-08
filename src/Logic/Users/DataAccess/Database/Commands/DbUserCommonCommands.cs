using System;
using System.Data;
using System.Threading;
using Dapper;
using Logic.Users.DataAccess.Database.Configuration;
using Logic.Users.Models;
using static Logic.Users.DataAccess.Database.Configuration.DatabaseNames.UsersColumns;

namespace Logic.Users.DataAccess.Database.Commands
{
    public static class DbUserCommonCommands
    {
        public static CommandDefinition CreateInsertCommand(User user, CancellationToken cancellationToken = default)
        {
            const string queryString = $@"
                INSERT INTO {DatabaseNames.UsersTable}
                ({Email}, {Phone}, {FIO}, {Password})
                VALUES (@Email, @Phone, @FIO, @Password)
                RETURNING {Id}";
            var command = new CommandDefinition(queryString, new
            {
                user.Email, user.Phone, user.FIO, user.Password
            }, commandType: CommandType.Text, cancellationToken: cancellationToken);
            return command;
        }

        public static CommandDefinition CreateCheckUniquePhoneCommand(string phone, CancellationToken cancellationToken)
        {
            const string queryString = $@"SELECT {Id} FROM {DatabaseNames.UsersTable} WHERE {Phone} = @phone LIMIT 1";
            var command = new CommandDefinition(queryString, new
            {
                phone
            }, commandType: CommandType.Text, cancellationToken: cancellationToken);
            return command;
        }

        public static CommandDefinition CreateCheckUniqueEmailCommand(string email, CancellationToken cancellationToken)
        {
            const string queryString = $@"SELECT {Id} FROM {DatabaseNames.UsersTable} WHERE {Email} = @email LIMIT 1";
            var command = new CommandDefinition(queryString, new
            {
                email
            }, commandType: CommandType.Text, cancellationToken: cancellationToken);
            return command;
        }

        public static CommandDefinition CreateFindUserByPhonePasswordCommand(string phone, string password, CancellationToken cancellationToken)
        {
            const string queryString = $@"SELECT * FROM {DatabaseNames.UsersTable} WHERE {Phone} = @phone AND {Password} = @password LIMIT 1";
            var command = new CommandDefinition(queryString, new
            {
                phone, password
            }, commandType: CommandType.Text, cancellationToken: cancellationToken);
            return command;
        }

        public static CommandDefinition CreateUpdateLastLoginCommand(int userId, DateTime lastLogin, CancellationToken cancellationToken)
        {
            const string queryString = $@"UPDATE {DatabaseNames.UsersTable}
                SET {LastLogin} = @lastLogin
                WHERE {Id} = @userId";
            var command = new CommandDefinition(queryString, new
            {
                userId, lastLogin
            }, commandType: CommandType.Text, cancellationToken: cancellationToken);
            return command;
        }

        public static CommandDefinition CreateFindUserByIdCommand(int id, CancellationToken cancellationToken)
        {
            const string queryString = $@"SELECT * FROM {DatabaseNames.UsersTable} WHERE {Id} = @id LIMIT 1";
            var command = new CommandDefinition(queryString, new
            {
                id
            }, commandType: CommandType.Text, cancellationToken: cancellationToken);
            return command;
        }
    }
}