namespace Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Domain.Entities
{
    public class Fatura
    {
        public int Id { get; set; }
        public int ContratoId {  get; set; }
        public string MesReferencia {  get; set; }
        public decimal Valor {  get; set; }
        public StatusFatura Status {  get; set; }
        public DateTime? DataVencimento {  get; set; }

    
        public enum StatusFatura
        {
            Aberta,
            Paga,
            Vencida,
            Baixada
        }
    }
}
  