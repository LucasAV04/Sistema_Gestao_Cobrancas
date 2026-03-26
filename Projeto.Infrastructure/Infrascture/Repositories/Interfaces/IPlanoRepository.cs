using Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Domain.Entities;

namespace Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Infrascture.Repositories.Interfaces
{
    public interface IPlanoRepository
    {
        void Adicionar(Plano plano);
        Plano BuscarPorId(int id);
        void Atualizar(Plano plano);
        List<Plano> Listar();
        void RemoverPlano(int id);
    }
}
