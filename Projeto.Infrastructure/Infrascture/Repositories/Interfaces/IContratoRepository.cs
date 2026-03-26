using Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Domain.Entities;

namespace Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Infrascture.Repositories.Interfaces
{
    public interface IContratoRepository
    {
        void CriarContrato(Contrato contrato);
        Contrato BuscarPorId(int id);
        List<Contrato> Listar();
        void Atualizar(Contrato contrato);
        void RemoverContrato(int id);
    }
}
