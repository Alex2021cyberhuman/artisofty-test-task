using System.Threading;
using Dapper;

namespace Logic.Users.DataAccess.Database.Commands
{
    public static class DbInitialCommands
    {
        public static CommandDefinition GetCreateDatabaseCommand(string databaseName, CancellationToken cancellationToken = default)
        {
            const string queryString = $@"CREATE DATABASE @databaseName;";
            var command = new CommandDefinition(queryString, new
            {
                databaseName
            }, cancellationToken: cancellationToken);
            return command;
        }
    }
}