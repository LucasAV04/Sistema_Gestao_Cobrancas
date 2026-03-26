

namespace Projeto.Infrastructure.Infrascture.Repositories.Interfaces
{
    public interface IPagamentosRepository
    {
        void RegistrarPagamento(Pagamento pagamento);
        List<Pagamento> Listar();
        List<Pagamento> ListarPagamentoFatura(int id);
        void Deletar(int id);
    }
}
