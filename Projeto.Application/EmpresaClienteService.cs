using Projeto.Infrastructure.Infrascture.Repositories.Interfaces;

namespace Projeto.Application
{
    public class EmpresaClienteService
    {
        private readonly IEmpresaClienteRepository _empRepo;

        public EmpresaClienteService(IEmpresaClienteRepository empRepo)
        {
            _empRepo = empRepo;
        }

        public void Adicionar(string razaoSocial,string cnpj,string email)
        {
            var existente = _empRepo.ListarTodas().Any(e => e.Cnpj == cnpj);
            if (existente)  
            {
                throw new Exception("Já Existe uma Empresa com esse Cnpj");
            }
            var empCliente = new EmpresaCliente
            {
                RazaoSocial = razaoSocial,
                Cnpj = cnpj,
                Email = email,
                Ativo = true,
                DataCadastro = DateTime.Now                
            };
            if(razaoSocial == null || cnpj == null || email == null)
                throw new Exception("Informações em Branco");
            _empRepo.Adicionar(empCliente);
        }
        public void AtualizarEmpresa(int id, string razaoSocial, string cnpj, string email)
        {
            var empresa = _empRepo.BuscarPorId(id);
            if(empresa == null)
            {
                throw new Exception("Empresa não Encontrada");
            }
            empresa.AtualizarDados(razaoSocial,cnpj,email);
            _empRepo.Atualizar(empresa);
        }
        public List<EmpresaCliente>ListarTodos()
        {
             return _empRepo.ListarTodas();
        }
        public EmpresaCliente BuscarPorId(int id)
        {
            return _empRepo.BuscarPorId(id);
        }
        public List<EmpresaCliente> ListarAtivos()
        {
            return _empRepo.ListarTodas().Where(a => a.Ativo).ToList();
        }

        public void Ativar(int id)
        {
            var empCliente = _empRepo.BuscarPorId(id);
            if(empCliente == null)
            {
                throw new Exception("Não Encontrada");
            }
            empCliente.Ativar();
            _empRepo.Atualizar(empCliente);
        }

        public void Desativar(int id)
        {
            var empCliente = _empRepo.BuscarPorId(id);
            if(empCliente == null)
            {
                throw new Exception("Não Encontrada");
            }
            empCliente.Desativar();
            _empRepo.Atualizar(empCliente);
        }

        public void RemoverEmpresaCliente(int id)
        {
            var empresa = _empRepo.BuscarPorId(id);
            if (empresa == null)
                throw new Exception("Empresa não Encontrada");
            _empRepo.RemoverEmpresaCliente(id);
        }
    }
}
