using Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Domain.Entities;
using Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Infrascture.Repositories.Interfaces;

namespace Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Infrascture.Repositories.Sqlite
{
    public class PagamentoSqlite:IPagamentosRepository
    {
        public void RegistrarPagamento(Pagamento pagamento)
        {
            using var connection = SqliteConnectionFactory.Create();
            using var command = connection.CreateCommand();

            command.CommandText = @"INSERT INTO Pagamento (FaturaId,ValorPago,DataPagamento,FormaPagamento)
                                    VALUES(@faturaId,@valorPago,@dataPagamento,@formaPagamento)";

            command.Parameters.AddWithValue("@faturaId", pagamento.FaturaId);
            command.Parameters.AddWithValue("@valorPago", pagamento.ValorPago);
            command.Parameters.AddWithValue("@dataPagamento", pagamento.DataPagamento.ToString("yyyy-MM-dd HH:mm:ss"));
            command.Parameters.AddWithValue("@formaPagamento", pagamento.FormaPagamento);
            command.ExecuteNonQuery();
        }

        public List<Pagamento> Listar()
        {
            using var connection = SqliteConnectionFactory.Create();
            using var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM Pagamento";

            using var reader = command.ExecuteReader();
            var lista = new List<Pagamento>();
            while (reader.Read())
            {
                string dataPagamentoStr = reader.GetString(3);
                DateTime dataPagamento = DateTime.Parse(dataPagamentoStr);
                lista.Add(new Pagamento
                {
                    Id = reader.GetInt32(0),
                    FaturaId = reader.GetInt32(1),
                    ValorPago = reader.GetDecimal(2),
                    DataPagamento = dataPagamento,
                    FormaPagamento = reader.GetString(4)
                });
            }
            return lista;
        }
        public List<Pagamento> ListarPagamentoFatura(int faturaId)
        {
            using var connection = SqliteConnectionFactory.Create();
            var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM Pagamento WHERE FaturaId = @faturaId";
            command.Parameters.AddWithValue("@faturaId", faturaId);

            using var reader = command.ExecuteReader();
            var lista = new List<Pagamento>();

            while (reader.Read())
            {
                string dataPagamentoStr = reader.GetString(3);
                DateTime dataPagamento = DateTime.Parse(dataPagamentoStr);

                lista.Add(new Pagamento
                {
                    Id = reader.GetInt32(0),
                    FaturaId = reader.GetInt32(1),
                    ValorPago = reader.GetDecimal(2),
                    DataPagamento = dataPagamento,
                    FormaPagamento = reader.GetString(4)
                });
            }

            return lista;
        }

        public void Deletar(int id)
        {
            using var connection = SqliteConnectionFactory.Create();
            var command = connection.CreateCommand();

            command.CommandText = @"DELETE FROM Pagamento WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);

            int linhasAfetadas = command.ExecuteNonQuery();
            if (linhasAfetadas == 0)
            {
                throw new Exception("Nenhum pagamento encontrado com esse Id.");
            }
        }

    }
}
