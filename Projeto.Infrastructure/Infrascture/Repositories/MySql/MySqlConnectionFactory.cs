using MySqlConnector;
using Microsoft.Extensions.Configuration;

namespace Projeto.Infrastructure.Infrascture.Repositories.MySql
{
    public class MySqlConnectionFactory
    {

        private readonly string _connectionString;

        public MySqlConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public MySqlConnection Create()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}