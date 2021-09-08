using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace Logic.Users.DataAccess.Database.Commands
{
    public static class DbInitialCommands
    {
        public static CommandDefinition CreateInitDatabaseCommand(string databaseName, CancellationToken cancellationToken = default)
        {
            var queryString =
                $@"CREATE DATABASE {databaseName}";
            var command = new CommandDefinition(queryString, cancellationToken: cancellationToken);
            return command;
        }

        public static CommandDefinition CreateCheckDatabaseCommand(string databaseName, CancellationToken cancellationToken = default)
        {
            const string queryString = 
                @"SELECT datname FROM pg_database where datname = @databaseName;";
            var command = new CommandDefinition(queryString, new
            {
                databaseName
            }, cancellationToken: cancellationToken);
            return command;
        }

        public static async Task CreateDatabaseAsync(this NpgsqlConnection connection, string databaseName, CancellationToken cancellationToken = default)
        {
            var checkCommand = CreateCheckDatabaseCommand(databaseName, cancellationToken);
            var result = await connection.QueryFirstOrDefaultAsync<string>(checkCommand);
            if (string.IsNullOrWhiteSpace(result))
            {
                var initCommand = CreateInitDatabaseCommand(databaseName, cancellationToken);
                await connection.ExecuteAsync(initCommand);
            }
        }
    }
}