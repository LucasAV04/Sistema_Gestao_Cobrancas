using Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Domain.Entities;
using Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Infrascture.Repositories.Interfaces;

namespace Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Infrascture.Repositories.Memory
{
    public class FaturaRepositoryMemory:IFaturaRepository
    {
        private int proximoId = 1;
        private readonly List<Fatura> _faturas = new();

        public void GerarFatura(Fatura fatura)
        {
            fatura.Id = proximoId++;
            _faturas.Add(fatura);
        }
        public Fatura BuscarPorId(int id)
        {
            return _faturas.FirstOrDefault(f => f.Id == id);
        }
        public void AtualizarFatura(Fatura fatura)
        {
            var index = _faturas.FindIndex(i => i.Id == fatura.Id);
            if (index == -1)
                throw new Exception("Não foi possível Atualizar a Fatura");
            _faturas[index] = fatura;
        }
        public List<Fatura> Listar()
        {
            return _faturas;
        }
        public void RemoverFatura(int id)
        {
            var fatura = _faturas.FirstOrDefault(f => f.Id == id);
            if (fatura == null)
                throw new Exception("Fatura não Encontrada");
            _faturas.Remove(fatura);
        }
    }
}
