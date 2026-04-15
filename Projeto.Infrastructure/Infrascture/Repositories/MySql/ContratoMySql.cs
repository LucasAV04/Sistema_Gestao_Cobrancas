using Projeto.Infrastructure.Infrascture.Repositories.Interfaces;
using Projeto.Domain;
using Projeto.Infrastructure.Infrascture.Repositories.MySql;
using Org.BouncyCastle.Crypto.Signers;
namespace Projeto.Infrastructure.Infrascture.Repositories.MySql
{
    public class ContratoMySql:IContratoRepository
    {
        private readonly  MySqlConnectionFactory _connectionFactory;

        public ContratoMySql(MySqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void CriarContrato(Contrato contrato)
        {
            using var connection = _connectionFactory.Create();
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = @"INSERT INTO Contrato 
    (EmpresaClienteId, PlanoId, DataInicio, DataFim, Status, DiaVencimento)
    VALUES (@empresaClienteId, @planoId, @dataInicio, @dataFim, @status, @diaVencimento)";

            command.Parameters.AddWithValue("@empresaClienteId", contrato.EmpresaClienteId);
            command.Parameters.AddWithValue("@planoId", contrato.PlanoId);
            command.Parameters.AddWithValue("@dataInicio", contrato.DataInicio);

            if (contrato.DataFim.HasValue)
                command.Parameters.AddWithValue("@dataFim", contrato.DataFim.Value);
            else
                command.Parameters.AddWithValue("@dataFim", DBNull.Value);

            command.Parameters.AddWithValue("@status", (int)contrato.Status);
            command.Parameters.AddWithValue("@diaVencimento", contrato.DiaVencimento);

            command.ExecuteNonQuery();

            contrato.Id = (int)command.LastInsertedId;

        }

        public Contrato BuscarPorId(int id)
        {
            using var connection = _connectionFactory.Create();
            connection.Open();
            using var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM Contrato WHERE Id = @id";
            command.Parameters.AddWithValue("@id",id);

            using var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            DateTime? dataFim = null;
            if (!reader.IsDBNull(reader.GetOrdinal("DataFim")))
            {
                string dataFimStr = reader.GetString(reader.GetOrdinal("DataFim"));
                dataFim = DateTime.Parse(dataFimStr);
            }
            int statusInt = reader.GetInt32(reader.GetOrdinal("Status"));
            Contrato.StatusContrato status = (Contrato.StatusContrato)statusInt;

            return new Contrato
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                EmpresaClienteId = reader.GetInt32(reader.GetOrdinal("EmpresaClienteId")),
                PlanoId = reader.GetInt32(reader.GetOrdinal("PlanoId")),
                DataInicio = reader.GetDateTime(reader.GetOrdinal("DataInicio")),
                DataFim = dataFim,
                Status = status,
                DiaVencimento = reader.GetInt32(reader.GetOrdinal("DiaVencimento"))
            };
        }

        public List<Contrato> Listar()
        {
            using var connection = _connectionFactory.Create();
            connection.Open();
            using var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM Contrato";
            using var reader = command.ExecuteReader();
            var lista = new List<Contrato>();

            while(reader.Read())
            {
                int statusInt = reader.GetInt32(reader.GetOrdinal("Status"));
                Contrato.StatusContrato status = (Contrato.StatusContrato)statusInt;
                DateTime? dataFim = null;
                if (!reader.IsDBNull(reader.GetOrdinal("DataFim")))
                {
                    string dataFimStr = reader.GetString(reader.GetOrdinal("DataFim"));
                    dataFim = DateTime.Parse(dataFimStr);
                }

                lista.Add(new Contrato
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    EmpresaClienteId = reader.GetInt32(reader.GetOrdinal("EmpresaClienteId")),
                    PlanoId = reader.GetInt32(reader.GetOrdinal("PlanoId")),
                    DataInicio = reader.GetDateTime(reader.GetOrdinal("DataInicio")),
                    DataFim = dataFim,
                    Status = status,
                    DiaVencimento = reader.GetInt32(reader.GetOrdinal("DiaVencimento"))
                });
            }
            return lista;
        }
        public void Atualizar(Contrato contrato)
        {
            using var connection = _connectionFactory.Create();
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText = @"UPDATE Contrato 
                            SET EmpresaClienteId = @empresaClienteId,
                                PlanoId = @planoId,
                                DataInicio = @dataInicio,
                                DataFim = @dataFim,
                                Status = @status,
                                DiaVencimento = @diaVencimento
                            WHERE Id = @id";
            command.Parameters.AddWithValue("@empresaClienteId", contrato.EmpresaClienteId);
            command.Parameters.AddWithValue("@planoId", contrato.PlanoId);
            command.Parameters.AddWithValue("@dataInicio", contrato.DataInicio);

            if (contrato.DataFim.HasValue)
                command.Parameters.AddWithValue("@dataFim", contrato.DataFim.Value);
            else
                command.Parameters.AddWithValue("@dataFim", DBNull.Value);

            command.Parameters.AddWithValue("@status", (int)contrato.Status);
            command.Parameters.AddWithValue("@diaVencimento", contrato.DiaVencimento);

            command.ExecuteNonQuery();

            contrato.Id = (int)command.LastInsertedId;
        }
        public void RemoverContrato(int id)
        {
            using var connection = _connectionFactory.Create();
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText = @"DELETE FROM Contrato WHERE Id = @id";
            int linhasAtiva = command.ExecuteNonQuery();
            if (linhasAtiva == 0)
                throw new Exception("Contrato não Encontrado");
        }
    }
}
