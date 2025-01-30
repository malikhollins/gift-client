using Microsoft.Data.SqlClient;
using System.Data;

namespace WebAPI.Services
{
    public class DapperConnectionService : IConnectionService
    {
        private const string connectionConfigKey = "SqlConnection";
        private readonly string _connectionString;

        public DapperConnectionService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(connectionConfigKey)!;
        }

        public IDbConnection EstablishConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
