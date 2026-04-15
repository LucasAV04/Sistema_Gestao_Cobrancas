using MySql.Data.MySqlClient;
using Projeto.Infrastructure.Infrascture.Repositories.Interfaces;

namespace Projeto.Infrastructure.Infrascture.Repositories.MySql
{
    public class EmpresaClienteMySql:IEmpresaClienteRepository
    {
        private readonly MySqlConnectionFactory _connectionFactory;
        public EmpresaClienteMySql(MySqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public void Adicionar(EmpresaCliente empresaCliente)
        {
            using var connection = _connectionFactory.Create();
            connection.Open();

            var command =  connection.CreateCommand();

            command.CommandText = @"INSERT INTO Empresa (RazaoSocial,Cnpj,Email,Ativo,DataCadastro)
                                  VALUES (@razaoSocial,@cnpj,@email,@ativo,@dataCadastro)";

            command.Parameters.AddWithValue("@razaoSocial",empresaCliente.RazaoSocial);
            command.Parameters.AddWithValue("@cnpj",empresaCliente.Cnpj);
            command.Parameters.AddWithValue("@email",empresaCliente.Email);
            command.Parameters.AddWithValue("@ativo",empresaCliente.Ativo);
            command.Parameters.AddWithValue("@dataCadastro",empresaCliente.DataCadastro);
            command.ExecuteNonQuery();
        }
        
        public EmpresaCliente BuscarPorId(int id)
        {
            using var connection = _connectionFactory.Create();
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM Empresa WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            return new EmpresaCliente
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                RazaoSocial = reader.GetString(reader.GetOrdinal("RazaoSocial")),
                Cnpj = reader.GetString(reader.GetOrdinal("Cnpj")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Ativo = reader.GetInt32(reader.GetOrdinal("Ativo")) == 1,
                DataCadastro = reader.GetDateTime(reader.GetOrdinal("DataCadastro"))
            };

        }

        public EmpresaCliente BuscarPorCnpj(string cnpj)
        {
            using var connection = _connectionFactory.Create();
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM Empresa WHERE Cnpj = @cnpj";
            command.Parameters.AddWithValue("@cnpj", cnpj);

            using var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
          

            return new EmpresaCliente
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                RazaoSocial = reader.GetString(reader.GetOrdinal("RazaoSocial")),
                Cnpj = reader.GetString(reader.GetOrdinal("Cnpj")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Ativo = reader.GetInt32(reader.GetOrdinal("Ativo")) == 1,
                DataCadastro = reader.GetDateTime(reader.GetOrdinal("DataCadastro"))
            };
        }
        public void Atualizar(EmpresaCliente empresaCliente)
        {
            using var connection = _connectionFactory.Create();
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText = @"UPDATE Empresa SET RazaoSocial = @razaoSocial,Cnpj = @cnpj,Email = @email,Ativo = @ativo WHERE Id = @id";

            command.Parameters.AddWithValue("@razaoSocial", empresaCliente.RazaoSocial);
            command.Parameters.AddWithValue("@cnpj", empresaCliente.Cnpj);
            command.Parameters.AddWithValue("@email", empresaCliente.Email);
            command.Parameters.AddWithValue("@ativo", empresaCliente.Ativo);
            command.Parameters.AddWithValue("@id", empresaCliente.Id);
            command.ExecuteNonQuery();
        }
        public List<EmpresaCliente> ListarTodas()
        {
            using var connection = _connectionFactory.Create();
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM Empresa";
            using var reader = command.ExecuteReader();
            var lista = new List<EmpresaCliente>();
           
            while (reader.Read())
            {
                lista.Add(new EmpresaCliente
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    RazaoSocial = reader.GetString(reader.GetOrdinal("RazaoSocial")),
                    Cnpj = reader.GetString(reader.GetOrdinal("Cnpj")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Ativo = reader.GetInt32(reader.GetOrdinal("Ativo")) == 1,
                    DataCadastro = reader.GetDateTime(reader.GetOrdinal("DataCadastro"))
                });
            }
            return lista;
        }
        public void RemoverEmpresaCliente(int id)
        {
            using var connection =  _connectionFactory.Create();
            connection.Open();
            using var command = connection.CreateCommand();

            command.CommandText = @"DELETE FROM Empresa WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);

            int linhasAfetadas = command.ExecuteNonQuery();
            if(linhasAfetadas == 0)
            {
                throw new Exception("Nenhuma Empresa Encontrada");
            }
        }
    }
}
