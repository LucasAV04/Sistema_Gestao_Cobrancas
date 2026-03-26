
namespace Projeto.Infrastructure.Infrascture.Repositories.Interfaces
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
