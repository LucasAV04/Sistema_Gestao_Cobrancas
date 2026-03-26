using Projeto.Infrastructure.Infrascture.Repositories.Interfaces;

namespace Projeto.Application
{
    public class ContratoService
    {
        private readonly IContratoRepository _contRepo;
        private readonly IEmpresaClienteRepository _empRepo;
        private readonly IPlanoRepository _plaRepo;

        public ContratoService(IContratoRepository contRepo, IEmpresaClienteRepository empRepo, IPlanoRepository plaRepo)
        {
            _contRepo = contRepo;
            _empRepo = empRepo;
            _plaRepo = plaRepo;
        }

        public void CriarContrato(int empresaId,int planoId,int diaVencimento)
        {
            var empresa = _empRepo.BuscarPorId(empresaId);
            if(empresa == null || !empresa.Ativo)
                throw new Exception("Empresa Inválida ou inativa");
            
            var plano = _plaRepo.BuscarPorId(planoId);
            if (plano == null || !plano.Ativo)
                throw new Exception("Plano Inválida ou inativa");

            var cont = new Contrato(empresaId,planoId,diaVencimento);
            _contRepo.CriarContrato(cont);
        }

        public void SuspenderContrato(int contratoId)
        {
            var contrato = _contRepo.BuscarPorId(contratoId);
            if (contrato == null)
                throw new Exception("Não foi possivel achar um contrato com esse Id");
            contrato.Status = Contrato.StatusContrato.Suspenso;
            _contRepo.Atualizar(contrato);
        }
        public void ReativarContrato(int contratoId)
        {
            var contrato = _contRepo.BuscarPorId(contratoId);
            if (contrato == null)
                throw new Exception("Não foi possível achar um contrato com esse Id");
            contrato.Status = Contrato.StatusContrato.Ativo;
            _contRepo.Atualizar(contrato);
        }
        public void CancelarContrato(int contratoId)
        {
            var contrato = _contRepo.BuscarPorId(contratoId);
            if (contrato == null)
                throw new Exception("Não foi possível achar um contrato com esse Id");
            contrato.Status = Contrato.StatusContrato.Cancelado;
            _contRepo.Atualizar(contrato);
        }
        public bool ValidarEmresaAtiva(int empresaId)
        {
            var empresa = _empRepo.BuscarPorId(empresaId);
            if (empresa == null)
                throw new Exception("Não foi possível achar uma Empresa com esse Id");
            if (empresa.Ativo = false)
                return false;
            return true;
        }
        public bool ValidarPlanoAtivo(int planoId)
        {
            var plano = _plaRepo.BuscarPorId(planoId);
            if (plano == null)
                throw new Exception("Não foi possível achar uma Empresa com esse Id");
            if(plano.Ativo = false) 
                return false;
            return true;
        }
        public List<Contrato> ListarContrato()
        {
            return _contRepo.Listar();
        }
        public void RemoverContrato(int id)
        {
            _contRepo.RemoverContrato(id);
        }
    }
}
