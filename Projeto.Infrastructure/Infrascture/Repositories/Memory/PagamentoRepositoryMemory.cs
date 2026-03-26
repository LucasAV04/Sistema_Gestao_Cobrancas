using Projeto.Infrastructure.Infrascture.Repositories.Interfaces;

namespace Projeto.Infrastructure.Infrascture.Repositories.Memory
{
    public class PagamentoRepositoryMemory:IPagamentosRepository
    {
        private int proximoId = 1;
        private readonly List<Pagamento> _pagamentos = new ();
        public void RegistrarPagamento(Pagamento pagamento)
        {
            pagamento.Id = proximoId++;
            _pagamentos.Add(pagamento);
        }

        public List<Pagamento> Listar()
        {
            return _pagamentos;
        }
        public List<Pagamento>ListarPagamentoFatura(int faturaId)
        {
            return _pagamentos.Where(p => p.FaturaId == faturaId).ToList();
        }
        public void Deletar(int id)
        {
            var pagamento = _pagamentos.FirstOrDefault(p => p.Id == id);
            if (pagamento == null)
                throw new Exception("Pagamento não Encontrado");
            _pagamentos.Remove(pagamento);
        }
    }
}
