

namespace Projeto.Infrastructure.Infrascture.Repositories.Interfaces
{
    public interface IFaturaRepository
    {
        void GerarFatura(Fatura fatura);
        Fatura BuscarPorId(int id);
        void AtualizarFatura(Fatura fatura);
        List<Fatura> Listar();
        void RemoverFatura(int id);
        
    }
}
