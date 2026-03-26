

using Microsoft.Data.Sqlite;

namespace Projeto.Infrastructure.Infrascture.Repositories.Sqlite
{
    public class SqliteConnectionFactory
    {
        private static readonly string connectionString = @"Data Source=C:\Users\Lucas\Desktop\SistemaGestãoCobramcas\Projeto.Infrastructure\GestaoContratoCobrancas.db";

        public static SqliteConnection Create()
        {
            var connection = new SqliteConnection(connectionString);
            connection.Open();  
            return connection;
        }
    }
}
