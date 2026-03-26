namespace Sistema_de_Gestão_de_Contratos_e_Cobranças.ProjetoCore.Domain.Entities
{
    public class Contrato
    {
        public int Id { get; set; }
        public int EmpresaClienteId { get; set; }
        public int PlanoId { get; set; }
        public DateTime DataInicio {  get; set; }
        public DateTime? DataFim { get; set; }
        public StatusContrato Status {  get; set; }
        public int DiaVencimento {  get; set; }

        public Contrato() { }

        public Contrato(int empresaCliente,int plano, int diaVencimento)
        {
            EmpresaClienteId = empresaCliente;
            PlanoId = plano;
            if (empresaCliente == 0 && plano == 0)
                throw new Exception("Empresa e Plano Tem que está Cadsatrado");
            
            DataInicio = DateTime.Now;
            Status = StatusContrato.Ativo;
            DiaVencimento = diaVencimento;
        }
        public enum StatusContrato
        {
            Ativo,
            Suspenso,
            Cancelado
        }
    }
}
