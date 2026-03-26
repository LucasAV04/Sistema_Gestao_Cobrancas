using Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Domain.Entities;
using Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Infrascture.Repositories.Interfaces;

namespace Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Infrascture.Repositories.Memory
{
    public class EmpresaClienteMemory:IEmpresaClienteRepository
    {
        private int prximoId = 1;
        private readonly List<EmpresaCliente> _empCliente = new();

        public void Adicionar(EmpresaCliente empresaCliente)
        {
            empresaCliente.Id = prximoId++;
            _empCliente.Add(empresaCliente);
        }
        public EmpresaCliente BuscarPorId(int id)
        {
            return _empCliente.FirstOrDefault(e => e.Id == id);
        }

        public EmpresaCliente BuscarPorCnpj(string cnpj)
        {
            return _empCliente.FirstOrDefault(e => e.Cnpj == cnpj);
        }

        public void Atualizar(EmpresaCliente empresaCliente)
        {
            var index = _empCliente.FindIndex(e => e.Id == empresaCliente.Id);
            if(index == -1)
            {
                throw new Exception("Empresa Cliente não Encontrado Para Atualização");
            }
            _empCliente[index] = empresaCliente;
        }
        public List<EmpresaCliente> ListarTodas()
        {
            return _empCliente;
        }
        public void RemoverEmpresaCliente(int id)
        {
            _empCliente.RemoveAll(e => e.Id == id);
        }
    }
}
