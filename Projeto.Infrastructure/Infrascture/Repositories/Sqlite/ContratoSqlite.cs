using Projeto.Infrastructure.Infrascture.Repositories.Interfaces;
using Projeto.Domain;
namespace Projeto.Infrastructure.Infrascture.Repositories.Sqlite
{
    public class ContratoSqlite:IContratoRepository
    {
        public void CriarContrato(Contrato contrato)
        {
            using var connection = SqliteConnectionFactory.Create();
            using var command = connection.CreateCommand();

            command.CommandText = @"INSERT INTO Contrato 
        (EmpresaClienteId, PlanoId, DataInicio, DataFim, Status, DiaVencimento)
        VALUES (@empresaClienteId, @planoId, @dataInicio, @dataFim, @status, @diaVencimento)";

            command.Parameters.AddWithValue("@empresaClienteId",contrato.EmpresaClienteId);
            command.Parameters.AddWithValue("@planoId",contrato.PlanoId);

            command.Parameters.AddWithValue("@dataInicio", contrato.DataInicio.ToString("yyyy-MM-dd HH:mm:ss"));
            if (contrato.DataFim.HasValue)
                command.Parameters.AddWithValue("@dataFim", contrato.DataFim.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            else
                command.Parameters.AddWithValue("@dataFim", DBNull.Value);
            command.Parameters.AddWithValue("@status", (int)contrato.Status);

            command.Parameters.AddWithValue("@diaVencimento", contrato.DiaVencimento);

            command.ExecuteNonQuery();

        }

        public Contrato BuscarPorId(int id)
        {
            using var connection = SqliteConnectionFactory.Create();
            using var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM Contrato WHERE Id = @id";
            command.Parameters.AddWithValue("@id",id);

            using var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            string dataInicioStr = reader.GetString(3);
            DateTime dataInicio = DateTime.Parse(dataInicioStr);

            DateTime? dataFim = null;
            if (!reader.IsDBNull(4))
            {
                string dataFimStr = reader.GetString(4);
                dataFim = DateTime.Parse(dataFimStr);
            }
            int statusInt = reader.GetInt32(5);
            Contrato.StatusContrato status = (Contrato.StatusContrato)statusInt;

            return new Contrato
            {
                Id = reader.GetInt32(0),
                EmpresaClienteId = reader.GetInt32(1),
                PlanoId = reader.GetInt32(2),
                DataInicio = dataInicio,
                DataFim = dataFim,
                Status = status,
                DiaVencimento = reader.GetInt32(6)
            };
        }

        public List<Contrato> Listar()
        {
            using var connection = SqliteConnectionFactory.Create();
            using var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM Contrato";
            using var reader = command.ExecuteReader();
            var lista = new List<Contrato>();

            while(reader.Read())
            {
                string dataInicioStr = reader.GetString(3);
                DateTime dataInicio = DateTime.Parse(dataInicioStr);

                DateTime? dataFim = null;
                if (!reader.IsDBNull(4))
                {
                    string dataFimStr = reader.GetString(4);
                    dataFim = DateTime.Parse(dataFimStr);
                }
                int statusInt = reader.GetInt32(5);
                Contrato.StatusContrato status = (Contrato.StatusContrato)statusInt;

                lista.Add(new Contrato
                {
                    Id = reader.GetInt32(0),
                    EmpresaClienteId = reader.GetInt32(1),
                    PlanoId = reader.GetInt32(2),
                    DataInicio = dataInicio,
                    DataFim = dataFim,
                    Status = status,
                    DiaVencimento = reader.GetInt32(6)
                });
            }
            return lista;
        }
        public void Atualizar(Contrato contrato)
        {
            using var connection = SqliteConnectionFactory.Create();
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
            command.Parameters.AddWithValue("@dataInicio", contrato.DataInicio.ToString("yyyy-MM-dd HH:mm:ss"));
            command.Parameters.AddWithValue("@dataFim", contrato.DataFim.HasValue
                ? contrato.DataFim.Value.ToString("yyyy-MM-dd HH:mm:ss")
                : DBNull.Value);
            command.Parameters.AddWithValue("@status", (int)contrato.Status);
            command.Parameters.AddWithValue("@diaVencimento", contrato.DiaVencimento);
            command.Parameters.AddWithValue("@id", contrato.Id);

            command.ExecuteNonQuery();
        }
        public void RemoverContrato(int id)
        {
            using var connection = SqliteConnectionFactory.Create();
            var command = connection.CreateCommand();

            command.CommandText = @"DELETE FROM Contrato WHERE Id = @id";
            int linhasAtiva = command.ExecuteNonQuery();
            if (linhasAtiva == 0)
                throw new Exception("Contrato não Encontrado");
        }
    }
}
