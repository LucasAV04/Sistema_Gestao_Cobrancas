

using Org.BouncyCastle.Asn1.Mozilla;
using Projeto.Infrastructure.Infrascture.Repositories.Interfaces;
using Projeto.Infrastructure.Infrascture.Repositories.MySql;

namespace Projeto.Infrastructure.Infrascture.Repositories.MySql;
    public class FaturaMySql:IFaturaRepository
    {
        private readonly MySqlConnectionFactory _connectionFactory;
        
        public FaturaMySql(MySqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void GerarFatura(Fatura fatura)
        {
            using var connection = _connectionFactory.Create();
            connection.Open();
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
            using var connection = _connectionFactory.Create();
            connection.Open();
            using var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM Fatura WHERE Id = @id";
            command.Parameters.AddWithValue("@id",id);
            
            using var reader = command.ExecuteReader();
            if (!reader.Read())
                return null;

            int statusInt = reader.GetInt32(4);
            Fatura.StatusFatura status = (Fatura.StatusFatura)statusInt;
           
            return new Fatura
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                ContratoId = reader.GetInt32(reader.GetOrdinal("ContratoId")),
                MesReferencia = reader.GetString(reader.GetOrdinal("MesReferencia")),
                Valor = reader.GetDecimal(reader.GetOrdinal("Valor")),
                Status = status,
                DataVencimento = reader.GetDateTime(reader.GetOrdinal("DataVencimento"))
            };
        }
        public void AtualizarFatura(Fatura fatura)
        {
            using var connection = _connectionFactory.Create();
            connection.Open();
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
            using var connection = _connectionFactory.Create();
            connection.Open();
            using var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM Fatura";
            using var reader = command.ExecuteReader();
            var lista = new List<Fatura>();
            while (reader.Read())
            {
                int statusInt = reader.GetInt32(4);
                Fatura.StatusFatura status = (Fatura.StatusFatura)statusInt;
                lista.Add(new Fatura
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    ContratoId = reader.GetInt32(reader.GetOrdinal("ContratoId")),
                    MesReferencia = reader.GetString(reader.GetOrdinal("MesReferencia")),
                    Valor = reader.GetDecimal(reader.GetOrdinal("Valor")),
                    Status = status,
                    DataVencimento = reader.GetDateTime(reader.GetOrdinal("DataVencimento"))
                });
            }
            return lista;
        }
        public void RemoverFatura(int id)
        {
            using var connection = _connectionFactory.Create();
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText = @"DELETE FROM Fatura WHERE Id = @id";
            int linhasAfetadas = command.ExecuteNonQuery();
            if (linhasAfetadas == 0)
                throw new Exception("Fatura não Encontrada");
        }
    }  

