
using Microsoft.Data.Sqlite;

namespace Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Infrascture.Repositories.Sqlite
{
    public class SqliteConnectionFactory
    {
        private static readonly string connectionString = @"Data Source=C:\Users\Lucas\Desktop\Curso_c#Net\Sistema de Gestão de Contratos e Cobranças\GestaoContratoCobrancas.db";

        public static SqliteConnection Create()
        {
            var connection = new SqliteConnection(connectionString);
            connection.Open();  
            return connection;
        }
    }
}
