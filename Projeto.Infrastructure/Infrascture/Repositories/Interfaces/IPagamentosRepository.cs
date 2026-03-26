using Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Domain.Entities;

namespace Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Infrascture.Repositories.Interfaces
{
    public interface IPagamentosRepository
    {
        void RegistrarPagamento(Pagamento pagamento);
        List<Pagamento> Listar();
        List<Pagamento> ListarPagamentoFatura(int id);
        void Deletar(int id);
    }
}
