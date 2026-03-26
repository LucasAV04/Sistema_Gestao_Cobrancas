using Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Domain.Entities;

namespace Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Infrascture.Repositories.Interfaces
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
