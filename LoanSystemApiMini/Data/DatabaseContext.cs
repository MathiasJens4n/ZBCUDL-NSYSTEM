using System.Data;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace LoanSystemApiMini.Data    
{
    public class DatabaseContext
    {
        private readonly string _connectionString;

        public DatabaseContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("connection");
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}