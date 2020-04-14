using DataAccess.Dapper.ConnectionProvider;
using Microsoft.Data.SqlClient;

namespace DataAccess.Dapper.ConnectionFactory
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly string connectionString;

        public ConnectionFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
