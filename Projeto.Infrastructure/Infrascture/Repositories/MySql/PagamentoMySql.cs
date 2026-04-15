

using Projeto.Infrastructure.Infrascture.Repositories.Interfaces;
using Projeto.Infrastructure.Infrascture.Repositories.MySql;

namespace Projeto.Infrastructure.Infrascture.Repositories.MySql
{
    public class PagamentoMySql:IPagamentosRepository
    {
        private readonly MySqlConnectionFactory _connectionFactory;

        public PagamentoMySql(MySqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public void RegistrarPagamento(Pagamento pagamento)
        {
            using var connection = _connectionFactory.Create();
            connection.Open();
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
            using var connection = _connectionFactory.Create();
            connection.Open();
            using var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM Pagamento";

            using var reader = command.ExecuteReader();
            var lista = new List<Pagamento>();
            while (reader.Read())
            {
                lista.Add(new Pagamento
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    FaturaId = reader.GetInt32(reader.GetOrdinal("FaturaId")),
                    ValorPago = reader.GetDecimal(reader.GetOrdinal("ValorPago")),
                    DataPagamento = reader.GetDateTime(reader.GetOrdinal("DataPagamento")),
                    FormaPagamento = reader.GetString(reader.GetOrdinal("FormaPagamento"))
                });
            }
            return lista;
        }
        public List<Pagamento> ListarPagamentoFatura(int faturaId)
        {
            using var connection = _connectionFactory.Create();
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM Pagamento WHERE FaturaId = @faturaId";
            command.Parameters.AddWithValue("@faturaId", faturaId);

            using var reader = command.ExecuteReader();
            var lista = new List<Pagamento>();

            while (reader.Read())
            {
              
                lista.Add(new Pagamento
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    FaturaId = reader.GetInt32(reader.GetOrdinal("FaturaId")),
                    ValorPago = reader.GetDecimal(reader.GetOrdinal("ValorPago")),
                    DataPagamento = reader.GetDateTime(reader.GetOrdinal("DataPagamento")),
                    FormaPagamento = reader.GetString(reader.GetOrdinal("FormaPagamento"))
                });
            }

            return lista;
        }

        public void Deletar(int id)
        {
            using var connection = _connectionFactory.Create();
            connection.Open();
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
