using Microsoft.Data.Sqlite;
using Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Domain.Entities;
using Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Infrascture.Repositories.Interfaces;
namespace Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Infrascture.Repositories.Sqlite
{
    public class EmpresaClienteSqlite:IEmpresaClienteRepository
    {
        public void Adicionar(EmpresaCliente empresaCliente)
        {
            using var connection = SqliteConnectionFactory.Create();
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
            using var connection = SqliteConnectionFactory.Create();
            var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM Empresa WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            string dataCadastroStr = reader.GetString(5);
            DateTime dataCadastro = DateTime.Parse(dataCadastroStr);

            return new EmpresaCliente
            {
                Id = reader.GetInt32(0),
                RazaoSocial = reader.GetString(1),
                Cnpj = reader.GetString(2),
                Email = reader.GetString(3),
                Ativo = reader.GetInt32(4) == 1,
                DataCadastro = dataCadastro
            };

        }

        public EmpresaCliente BuscarPorCnpj(string cnpj)
        {
            using var connection = SqliteConnectionFactory.Create();
            var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM Empresa WHERE Cnpj = @cnpj";
            command.Parameters.AddWithValue("@cnpj", cnpj);

            using var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            string dataCadastroStr = reader.GetString(5);
            DateTime dataCadastro = DateTime.Parse(dataCadastroStr);

            return new EmpresaCliente
            {
                Id = reader.GetInt32(0),
                RazaoSocial = reader.GetString(1),
                Cnpj = reader.GetString(2),
                Email = reader.GetString(3),
                Ativo = reader.GetInt32(4) == 1,
                DataCadastro = dataCadastro
            };
        }
        public void Atualizar(EmpresaCliente empresaCliente)
        {
            using var connection = SqliteConnectionFactory.Create();
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
            using var connection = SqliteConnectionFactory.Create();
            var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM Empresa";
            using var reader = command.ExecuteReader();
            var lista = new List<EmpresaCliente>();
           
            while (reader.Read())
            {
                string dataCadastroStr = reader.GetString(5);
                DateTime dataCadastro = DateTime.Parse(dataCadastroStr);
                lista.Add(new EmpresaCliente
                {
                    Id = reader.GetInt32(0),
                    RazaoSocial = reader.GetString(1),
                    Cnpj = reader.GetString(2),
                    Email = reader.GetString(3),
                    Ativo = reader.GetInt32(4) == 1,
                    DataCadastro = dataCadastro
                });
            }
            return lista;
        }
        public void RemoverEmpresaCliente(int id)
        {
            using var connection = SqliteConnectionFactory.Create();
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
