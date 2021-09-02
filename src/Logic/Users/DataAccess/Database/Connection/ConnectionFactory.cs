using System;
using Logic.Users.DataAccess.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Logic.Users.DataAccess.Database.Connection
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IOptionsMonitor<DataAccessOptions> _optionsMonitor;

        public ConnectionFactory(IOptionsMonitor<DataAccessOptions> optionsMonitor)
        {
            _optionsMonitor = optionsMonitor;
        }

        public NpgsqlConnection CreateConnection()
        {
            var actualOptions = _optionsMonitor.CurrentValue;
            if (!actualOptions.UseDatabase)
                throw new InvalidOperationException();
            var connectionString = actualOptions.DatabaseOptions.ConnectionString;
            var connection = new NpgsqlConnection(connectionString);
            return connection;
        }
    }
}