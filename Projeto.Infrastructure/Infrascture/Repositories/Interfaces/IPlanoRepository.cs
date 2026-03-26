

namespace Projeto.Infrastructure.Infrascture.Repositories.Interfaces
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
