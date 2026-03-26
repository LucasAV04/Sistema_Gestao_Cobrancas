using Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Domain.Entities;

namespace Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Infrascture.Repositories.Interfaces
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
