using Microsoft.Data.SqlClient;

namespace DataAccess.Dapper.ConnectionProvider
{
    public interface IConnectionFactory
    {
        SqlConnection GetConnection();
    }
}
