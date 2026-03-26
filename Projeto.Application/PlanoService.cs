

using Projeto.Infrastructure.Infrascture.Repositories.Interfaces;

namespace Projeto.Application
{
    public class PlanoService
    {
        private readonly IPlanoRepository _planoRepo;

        public PlanoService(IPlanoRepository plano)
        {
            _planoRepo = plano;
        }

        public void Adicionar(string nome, decimal valorMensal, int limiteUsuario)
        {
            var plano = new Plano
            {
                Nome = nome,
                ValorMensal = valorMensal,
                LimiteUsuarios = limiteUsuario,
                Ativo = true
            };
            _planoRepo.Adicionar(plano);
        }

        public void AtualizarPlano(int id, string nome, decimal valorMensal, int limiteUsuario)
        {
            var plano = _planoRepo.BuscarPorId(id);
            if (plano == null)
            {
                throw new Exception("Plano não Encontrado para Atualização");
            }
            plano.AtualizarDados(nome,valorMensal,limiteUsuario);
            _planoRepo.Atualizar(plano);
        }
        public List<Plano> ListarTudo()
        {
            return _planoRepo.Listar();
        }

        public List<Plano>ListarAtivos()
        {
            return _planoRepo.Listar().Where(p =>p.Ativo).ToList();
        }

        public void Ativar(int id)
        {
            var existente = _planoRepo.BuscarPorId(id);
            if(existente == null)
            {
                throw new Exception("Não foi achado o Plano por esse Id");
            }
            existente.Ativar();
            _planoRepo.Atualizar(existente);
        }
        public void Desativar(int id)
        {
            var existente = _planoRepo.BuscarPorId(id);
            if (existente == null)
            {
                throw new Exception("Não foi achado o Plano por esse Id");
            }
            existente.Desativar();
            _planoRepo.Atualizar(existente);
        }
       public void RemoverPlano(int id)
        {
            _planoRepo.RemoverPlano(id);
        }
    }
}
