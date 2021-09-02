using Npgsql;

namespace Logic.Users.DataAccess.Database.Connection
{
    public interface IConnectionFactory
    {
        /// <summary>
        /// Creates connection to PostgreSql database.
        /// This method isn't opens connections.
        /// </summary>
        /// <returns></returns>
        NpgsqlConnection CreateConnection();
    }
}