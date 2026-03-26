
using Projeto.Infrastructure.Infrascture.Repositories.Interfaces;

namespace Projeto.Infrastructure.Infrascture.Repositories.Sqlite
{
    public class PlanoSqlite:IPlanoRepository
    {
        public void Adicionar(Plano plano)
        {
            using var connection = SqliteConnectionFactory.Create();
            using var command = connection.CreateCommand();

            command.CommandText = @"INSERT INTO Plano (Nome,ValorMensal,LimiteUsuarios,Ativo)
                                    VALUES (@nome,@valorMensal,@limiteUsuarios,@ativo)";

            command.Parameters.AddWithValue("@nome",plano.Nome);
            command.Parameters.AddWithValue("@valorMensal",plano.ValorMensal);
            command.Parameters.AddWithValue("@limiteUsuarios",plano.LimiteUsuarios);
            command.Parameters.AddWithValue("@ativo",plano.Ativo);
            command.ExecuteNonQuery();
        }

        public Plano BuscarPorId(int id)
        {
            using var connection = SqliteConnectionFactory.Create();
            using var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM Plano WHERE Id = @id";
            command.Parameters.AddWithValue("@id",id);

            using var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            return new Plano
            {
                Id = reader.GetInt32(0),
                Nome = reader.GetString(1),
                ValorMensal = reader.GetDecimal(2),
                LimiteUsuarios = reader.GetInt32(3),
                Ativo = reader.GetInt32(4) == 1
            };
        }

        public void Atualizar(Plano plano)
        {
            using var connection = SqliteConnectionFactory.Create();
            using var command = connection.CreateCommand();

            command.CommandText = @"UPDATE Plano SET Nome = @nome,ValorMensal = @valorMensal,LimiteUsuarios = @limiteUsuarios,Ativo = @ativo WHERE Id = @id";

            command.Parameters.AddWithValue("@nome", plano.Nome);
            command.Parameters.AddWithValue("@valorMensal", plano.ValorMensal);
            command.Parameters.AddWithValue("@limiteUsuarios", plano.LimiteUsuarios);
            command.Parameters.AddWithValue("@ativo", plano.Ativo);
            command.Parameters.AddWithValue("@id", plano.Id);
            command.ExecuteNonQuery();
        }

        public List<Plano> Listar()
        {
            using var connection = SqliteConnectionFactory.Create();
            using var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM Plano";
            using var reader = command.ExecuteReader();
            var lista = new List<Plano>();
            while (reader.Read())
            {
                lista.Add(new Plano
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    ValorMensal = reader.GetDecimal(2),
                    LimiteUsuarios = reader.GetInt32(3),
                    Ativo = reader.GetInt32(4) == 1
                });
            }
            return lista;
        }
        public void RemoverPlano(int id)
        {
            using var connection = SqliteConnectionFactory.Create();
            using var command = connection.CreateCommand();

            command.CommandText = @"DELETE FROM Plano WHERE Id = @id";
            int linhasAfetadas = command.ExecuteNonQuery();
            if (linhasAfetadas == 0)
                throw new Exception("Plano não Encontrado");
        }
    }
}
