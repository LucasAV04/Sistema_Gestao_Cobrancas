using Projeto.Infrastructure.Infrascture.Repositories.Interfaces;

namespace Projeto.Infrastructure.Infrascture.Repositories.Memory
{
    public class PlanoRepositoryMemory:IPlanoRepository
    {
       private readonly List<Plano> _planos = new();
        private int proximoId = 1;

        public void Adicionar(Plano plano)
        {
          
            plano.Id = proximoId++;
            _planos.Add(plano);
        }
        public Plano BuscarPorId(int id)
        {
            return _planos.FirstOrDefault(p => p.Id == id);
        }
        public void Atualizar(Plano plano)
        {
            var index = _planos.FindIndex(p => p.Id == plano.Id);
            if(index == -1)
            {
                throw new Exception("Plano não Encontrado para Atualização");
            }
            _planos[index] = plano;
        }
        public List<Plano> Listar()
        {
            return _planos;
        }
        public void RemoverPlano(int id)
        {
            var plano = _planos.FirstOrDefault(p => p.Id == id);
            if (plano == null)
                throw new Exception("O Plano não Foi achado");
            _planos.Remove(plano);
        }
    }
}
