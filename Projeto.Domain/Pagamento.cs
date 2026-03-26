namespace Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Domain.Entities
{
    public class Pagamento
    {
        public int Id {  get; set; }
        public int FaturaId {  get; set; }
        public decimal ValorPago {  get; set; }
        public DateTime DataPagamento {  get; set; }
        public string FormaPagamento { get; set; }
    }
}
