using Projeto.Infrastructure.Infrascture.Repositories.Interfaces;

namespace Projeto.Application
{
    public class FaturaService
    {
        private readonly IFaturaRepository _fatRepo;
        private readonly IContratoRepository _contRepo;
        private readonly IPlanoRepository _planoRepo;

        public FaturaService(IFaturaRepository fatRepo, IContratoRepository contRepo,IPlanoRepository planoRepo)
        {
            _fatRepo = fatRepo;
            _contRepo = contRepo;
            _planoRepo = planoRepo;
        }

        public void GerarFaturaContrato(int contratoId, int mes,int ano)
        {
           var contrato = _contRepo.BuscarPorId(contratoId);
            if (contrato == null)
                throw new Exception("Contrato não Achado");
            if (contrato.Status != Contrato.StatusContrato.Ativo)
                throw new Exception("Contrato não está Ativo");

            var plano = _planoRepo.BuscarPorId(contrato.PlanoId);
            if (plano == null)
                throw new Exception("Plano não Encontrato");
            if (plano.Ativo == false)
                throw new Exception("Plano não está Ativo");
            decimal valor = plano.ValorMensal;
            string mesReferencia = $"{mes:D2}/{ano}";
            if (ExisteFatura(contratoId, mesReferencia))
                throw new Exception("Fatura já gerada para este mês.");
            DateTime dataVencimento = new DateTime(ano, mes, contrato.DiaVencimento);
            var fatura = new Fatura
            {
                ContratoId = contratoId,
                MesReferencia = $"{mes:D2}/{ano}",
                Valor = valor,
                DataVencimento = dataVencimento,
                Status = Fatura.StatusFatura.Aberta

            };
            _fatRepo.GerarFatura(fatura);

        }
        public void GerarFaturasMensais()
        {
            int mesAtual = DateTime.Now.Month;
            int anoAtual = DateTime.Now.Year;

            var listaContratos = _contRepo.Listar();
            if (listaContratos == null || !listaContratos.Any())
                throw new Exception("Não há contratos cadastrados.");

            var listaPlanos = _planoRepo.Listar();
            if (listaPlanos == null || !listaPlanos.Any())
                throw new Exception("Não há planos cadastrados.");

            var contratosAtivos = listaContratos
                .Where(c => c.Status == Contrato.StatusContrato.Ativo)
                .ToList();

            if (!contratosAtivos.Any())
                throw new Exception("Não há contratos ativos para gerar faturas.");

            foreach (var contrato in contratosAtivos)
            {
                GerarFaturaContrato(contrato.Id, mesAtual, anoAtual);
            }

        }



        public void MarcarFaturaComoVencida(int faturaId)
        {
            var fatura = _fatRepo.BuscarPorId(faturaId);
            if (fatura == null)
                throw new Exception("Fatura não encontrada");
            fatura.Status = Fatura.StatusFatura.Vencida;
            _fatRepo.AtualizarFatura(fatura);
           
        }
        public List<Fatura> ListarFaturasPorContrato(int contratoId)
        {
            var contrato = _contRepo.BuscarPorId(contratoId);
            if (contrato == null)
                throw new Exception("Não foi achado o contrato");
            return _fatRepo.Listar().Where(f=> f.ContratoId == contratoId).ToList();
        }
        public List<Fatura> ListarFaturasEmAberto()
        {
            return _fatRepo.Listar().Where(f => f.Status == Fatura.StatusFatura.Aberta).ToList();
        }
        public void RemoverFatura(int id)
        {
            _fatRepo.RemoverFatura(id);
        }
        public bool ExisteFatura(int contratoId, string mesReferencia)
        {
            var lista = _fatRepo.Listar();

            return lista.Any(f =>
                f.ContratoId == contratoId &&
                f.MesReferencia == mesReferencia);
        }
    }
}
