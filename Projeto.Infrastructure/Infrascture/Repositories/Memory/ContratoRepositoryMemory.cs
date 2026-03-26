

using Projeto.Infrastructure.Infrascture.Repositories.Interfaces;

namespace Projeto.Infrastructure.Infrascture.Repositories.Memory
{
    public class ContratoRepositoryMemory : IContratoRepository
    {
        private int proximoId = 1;
        private readonly List<Contrato> _contratos = new();
        public void CriarContrato(Contrato contrato)
        {
            contrato.Id = proximoId++;
            _contratos.Add(contrato);
        }

        public Contrato BuscarPorId(int id)
        {
            return _contratos.FirstOrDefault(c => c.Id == id);
        }

        public List<Contrato> Listar()
        {
            return _contratos;
        }

        public void Atualizar(Contrato contrato)
        {
            var existente = _contratos.FirstOrDefault(c => c.Id == contrato.Id);
            if (existente != null)
            {
                existente.EmpresaClienteId = contrato.EmpresaClienteId;
                existente.PlanoId = contrato.PlanoId;
                existente.DataInicio = contrato.DataInicio;
                existente.DataFim = contrato.DataFim;
                existente.Status = contrato.Status;
                existente.DiaVencimento = contrato.DiaVencimento;
            }
        }

        public void RemoverContrato(int id)
        {
            var contrato = _contratos.FirstOrDefault(c => c.Id == id);
            if (contrato == null)
                throw new Exception("Contrato não ENcontrado");
            _contratos.Remove(contrato);
        }
    }
}
