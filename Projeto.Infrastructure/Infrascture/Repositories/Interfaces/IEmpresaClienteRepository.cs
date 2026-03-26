

namespace Projeto.Infrastructure.Infrascture.Repositories.Interfaces
{
    public interface IEmpresaClienteRepository
    {
        void Adicionar(EmpresaCliente empresaCliente);
        EmpresaCliente BuscarPorId(int id);
        EmpresaCliente BuscarPorCnpj(string cnpj);
        void Atualizar(EmpresaCliente empresaCliente);
        List<EmpresaCliente> ListarTodas();
        void RemoverEmpresaCliente(int id);
    }
}
