using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Projeto.Infrastructure.Infrascture.Repositories.Interfaces;
using System.Reflection.Metadata;
using Document = QuestPDF.Fluent.Document;

namespace Projeto.Application
{
    public class PagamentoService
    {
        private readonly IPagamentosRepository _pagRepo;
        private readonly IFaturaRepository _fatRepo;

        public PagamentoService(IPagamentosRepository pagRepo, IFaturaRepository fatRepo)
        {
            _pagRepo = pagRepo;
            _fatRepo = fatRepo;
        }

        public void RegistrarPagamento(int faturaId,decimal valor,string forma)
        {
            ValidarPagamentoDuplicado(faturaId);
            var fatura = _fatRepo.BuscarPorId(faturaId);
            if (fatura == null && fatura.Status == Fatura.StatusFatura.Vencida)
                throw new Exception("Fatura não Encontrada ou fatura já está vencida");
            var pagamento = new Pagamento
            {
                FaturaId = fatura.Id,
                ValorPago = valor,
                DataPagamento = DateTime.Now,
                FormaPagamento = forma
            };
            if (fatura.Valor != pagamento.ValorPago)
                throw new Exception("Valor Insuficiente");
            _pagRepo.RegistrarPagamento(pagamento);
            fatura.Status = Fatura.StatusFatura.Paga;
            _fatRepo.AtualizarFatura(fatura);
        }
        
        public List<Pagamento> Listar()
        {
            return _pagRepo.Listar();
        }
        public void BaixarFatura(int faturaId)
        {
            var fatura = _fatRepo.BuscarPorId(faturaId);
            if (fatura == null)
                throw new Exception("Fatura não encontrada");
            if (fatura.Status != Fatura.StatusFatura.Paga)
                throw new Exception("Fatura não pode ser baixada porque não foi paga");
            fatura.Status = Fatura.StatusFatura.Baixada;
            _fatRepo.AtualizarFatura(fatura);
        }
        public void ValidarPagamentoDuplicado(int faturaId)
        {
            var pagamento = _pagRepo.ListarPagamentoFatura(faturaId);
            if (pagamento.Any())
                throw new Exception("Já existe pagamento registrado para esta fatura.");
        }
        public void RemoverPagamento(int id)
        {
           _pagRepo.Deletar(id);
        }
        public byte[] GerarBoletoPdf(int id)
        {
            var pagamento= _pagRepo.Listar().FirstOrDefault(p => p.Id == id);
            if (pagamento == null)
                throw new Exception("Pagamento não encontrada");
            var fatura = _fatRepo.Listar().FirstOrDefault(f => f.Id == pagamento.FaturaId);
            if (fatura == null)
                throw new Exception("Fatura não encontrada");

            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text($"Fatura #{fatura.Id}")
                        .SemiBold().FontSize(20).AlignCenter();

                    page.Content()
                        .Column(col =>
                        {
                            col.Item().Text($"Contrato ID: {fatura.ContratoId}");
                            col.Item().Text($"Mês Referência: {fatura.MesReferencia}");
                            col.Item().Text($"Valor: R$ {fatura.Valor}");
                            col.Item().Text($"Status: {fatura.Status}");
                            col.Item().Text($"Data Vencimento: {fatura.DataVencimento:dd/MM/yyyy}");
                            col.Item().LineHorizontal(1);
                            col.Item().Text($"Pagamento ID: {pagamento.Id}");
                            col.Item().Text($"Valor Pago: R$ {pagamento.ValorPago}");
                            col.Item().Text($"Forma de Pagamento: {pagamento.FormaPagamento}");
                            col.Item().Text($"Data do Pagamento: {pagamento.DataPagamento:dd/MM/yyyy}");

                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Gerado em: ");
                            x.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        });
                });
            });

            return documento.GeneratePdf(); // retorna os bytes do PDF
        }

    }
}
