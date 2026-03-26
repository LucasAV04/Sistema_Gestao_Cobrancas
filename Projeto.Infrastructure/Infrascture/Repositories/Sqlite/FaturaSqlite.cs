

using Projeto.Infrastructure.Infrascture.Repositories.Interfaces;

namespace Projeto.Infrastructure.Infrascture.Repositories.Sqlite;
    public class FaturaSqlite:IFaturaRepository
    {
        public void GerarFatura(Fatura fatura)
        {
            using var connection = SqliteConnectionFactory.Create();
            using var command = connection.CreateCommand();

            command.CommandText = @"INSERT INTO Fatura(ContratoId,MesReferencia,Valor,Status,DataVencimento)
                                    VALUES(@contratoId,@mesReferencia,@valor,@status,@dataVencimento)";

            command.Parameters.AddWithValue("@contratoId", fatura.ContratoId);
            command.Parameters.AddWithValue("@mesReferencia", fatura.MesReferencia);
            command.Parameters.AddWithValue("@valor", fatura.Valor);
            command.Parameters.AddWithValue("@status", (int)fatura.Status);
            if (fatura.DataVencimento.HasValue)
                command.Parameters.AddWithValue("@dataVencimento", fatura.DataVencimento.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            else
                command.Parameters.AddWithValue("@dataVencimento",DBNull.Value);
            command.ExecuteNonQuery();
        }
        
        public Fatura BuscarPorId(int id)
        {
            using var connection = SqliteConnectionFactory.Create();
            using var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM Fatura WHERE Id = @id";
            command.Parameters.AddWithValue("@id",id);
            
            using var reader = command.ExecuteReader();
            if (!reader.Read())
                return null;

            int statusInt = reader.GetInt32(4);
            Fatura.StatusFatura status = (Fatura.StatusFatura)statusInt;
            DateTime? dataVencimento = null;
            if (!reader.IsDBNull(5))
            {
                string dataVencimentoStr = reader.GetString(5);
                dataVencimento = DateTime.Parse(dataVencimentoStr);  
            }
            return new Fatura
            {
                Id = reader.GetInt32(0),
                ContratoId = reader.GetInt32(1),
                MesReferencia = reader.GetString(2),
                Valor = reader.GetDecimal(3),
                Status = status,
                DataVencimento = dataVencimento
            };
        }
        public void AtualizarFatura(Fatura fatura)
        {
            using var connection = SqliteConnectionFactory.Create();
            using var command = connection.CreateCommand();

            command.CommandText = @"UPDATE Fatura SET ContratoId = @contratoId,MesReferencia = @mesReferencia,
                                    Valor = @valor, Status = @status,DataVencimento = @dataVencimento WHERE Id = @id";
            command.Parameters.AddWithValue("@contratoId",fatura.ContratoId);
            command.Parameters.AddWithValue("@mesReferencia",fatura.MesReferencia);
            command.Parameters.AddWithValue("@valor",fatura.Valor);
            command.Parameters.AddWithValue("@status",(int)fatura.Status);
            command.Parameters.AddWithValue("@dataVencimento", fatura.DataVencimento.HasValue
               ? fatura.DataVencimento.Value.ToString("yyyy-MM-dd HH:mm:ss")
               : DBNull.Value);
            command.Parameters.AddWithValue("@id", fatura.Id);
            command.ExecuteNonQuery();
        }
        public List<Fatura> Listar()
        {
            using var connection = SqliteConnectionFactory.Create();
            using var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM Fatura";
            using var reader = command.ExecuteReader();
            var lista = new List<Fatura>();
            while (reader.Read())
            {
                int statusInt = reader.GetInt32(4);
                Fatura.StatusFatura status = (Fatura.StatusFatura)statusInt;
                DateTime? dataVencimento = null;
                if (!reader.IsDBNull(5))
                {
                    string dataVencimentoStr = reader.GetString(5);
                    dataVencimento = DateTime.Parse(dataVencimentoStr);
                }
                lista.Add(new Fatura
                {

                    Id = reader.GetInt32(0),
                    ContratoId = reader.GetInt32(1),
                    MesReferencia = reader.GetString(2),
                    Valor = reader.GetDecimal(3),
                    Status = status,
                    DataVencimento = dataVencimento
                });
            }
            return lista;
        }
        public void RemoverFatura(int id)
        {
            using var connection = SqliteConnectionFactory.Create();
            var command = connection.CreateCommand();

            command.CommandText = @"DELETE FROM Fatura WHERE Id = @id";
            int linhasAfetadas = command.ExecuteNonQuery();
            if (linhasAfetadas == 0)
                throw new Exception("Fatura não Encontrada");
        }
    }  

